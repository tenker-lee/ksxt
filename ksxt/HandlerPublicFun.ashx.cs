using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;

namespace ksxt
{
    /// <summary>
    /// HandlerPublicFun 的摘要说明
    /// </summary>
    public class HandlerPublicFun : HandleBase, IHttpHandler, IRequiresSessionState
    {
        //private string user_id = "";
        //private string logonName = "";
        //private string logonType = "";

        //protected string ReadFormStr(HttpContext context, string itemName)
        //{
        //    if (context.Request.Form[itemName] == null)
        //        return "";
        //    else
        //        return context.Request.Form[itemName];
        //}

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
            /*
            try {
                user_id = context.Session["logonId"].ToString();
                logonName = context.Session["logonUser"] == null ? "" : context.Session["logonUser"].ToString();
                logonType = context.Session["logonUserType"] == null ? "" : context.Session["logonUser"].ToString();
            }
            catch (Exception ex) {
                WriteResponse(context, -1, "请重新登录", "");
                return;
            }

            string opt = context.Request.QueryString["opt"];
            if (opt == null)
                opt = "";

            Type thisType = this.GetType();
            MethodInfo method = thisType.GetMethod(opt, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
                WriteResponse(context, 0, "hello", "");
            else
                method.Invoke(this, new object[] { context });
                */
        }

        private void GetDateTime(HttpContext context)
        {
            string dateStr = publicFun.GetDateString(DateTime.Now);
            WriteResponse(context, 0, "查询成功", "\"now\":\"" + dateStr + "\"");
        }

        //private void WriteResponse(HttpContext context, int stateCode, string msg = "操作成功", string usrStr = "")
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    context.Response.Clear();

        //    stringBuilder.Append("{");
        //    stringBuilder.Append("\"stateCode\":");
        //    stringBuilder.Append(stateCode.ToString());
        //    stringBuilder.Append(",");

        //    stringBuilder.Append("\"msg\":\"");
        //    stringBuilder.Append(msg);
        //    stringBuilder.Append("\"");

        //    if (usrStr != null && usrStr != "") {
        //        stringBuilder.Append(",");
        //        stringBuilder.Append(usrStr);
        //    }

        //    stringBuilder.Append("}");

        //    context.Response.Write(stringBuilder.ToString());
        //}

        private void GetLogonInfo(HttpContext context)
        {
            string logonUser = context.Session["logonUser"] == null ? "" : context.Session["logonUser"].ToString();
            string logonUserType = context.Session["logonUserType"] == null ? "" : context.Session["logonUser"].ToString();
            WriteResponse(context, 0, "查询成功", "\"logonUser\":\"" + logonUser + "\",\"logonUserType\":\"" + logonUserType + "\"");
        }

        private void SetLononInfo(HttpContext context)
        {
            if (context.Request.QueryString["logonUser"] != null)
                context.Session["logonUser"] = context.Request.QueryString["logonUser"].ToString();
            if (context.Request.QueryString["logonUserType"] != null)
                context.Session["logonUserType"] = context.Request.QueryString["logonUserType"].ToString();
            WriteResponse(context, 0, "操作成功", "");
        }               

        private void UpdateAnswerList(HttpContext context)
        {
            //if (ExecuteQueryDataCount(@"SELECT * from tb_papers WHERE  end_time>'2019-04-26 0:0:0'") < 1) {
            //    WriteResponse(context, -1, "考试时间已过,请交卷!");
            //}
            string answerStr = ReadFormStr(context, "answerStr");
            //输入文本
            string answer = ReadFormStr(context, "value");
            string type="";
            string titleid="";
            string userid = "";
            string value ="";
            string valuetype = "";
            string regStr = "(?<type>\\S+)_(?<titleid>\\d+)_(?<userid>\\d+)_(?<valtype>answer|score)(_(?<value>\\d+))?";
            MatchCollection matchCollection = Regex.Matches(answerStr, regStr);
            if (matchCollection.Count > 0) {
                type = matchCollection[0].Groups["type"].Value;
                titleid = matchCollection[0].Groups["titleid"].Value;
                userid = matchCollection[0].Groups["userid"].Value;
                value = matchCollection[0].Groups["value"].Value;
                valuetype= matchCollection[0].Groups["valtype"].Value;
            }                                
            if (logonId != "") {
                if (valuetype == "score") {
                    UpdateScore(context, titleid, userid, answer);
                    return;
                }
                else {                    
                    if (type == "choice") {
                        InsertAnswerChoice(context, titleid, userid, value);
                    }
                    else if (type == "judge") {
                        InsertAnswerJudge(context, titleid, userid, value);
                    }
                    else if (type == "filling") {
                        InsertAnswerFilling(context, titleid, userid, value, answer);
                    }
                    else if (type == "qa") {
                        InsertAnswerQa(context, titleid, userid, answer);
                    }
                }
            }
            else {
                WriteResponse(context, -1, "登录超时,请重新登录进行操作!");
            }
           
        } 
        
        private void InsertAnswerChoice(HttpContext context,string title_list_id,string user_id,string value)
        {
            DataTable dt =  ExecuteQueryData(string.Format(@"SELECT p.choice_score,c.answer_arry
                                                FROM tb_title_list as t
                                                INNER JOIN tb_choice as c on t.title_id = c.id
                                                INNER JOIN tb_papers as p on p.id = t.paper_id
                                                WHERE t.type = 'choice' and t.id ={0}", title_list_id));
            int score = 0;
            if (dt.Rows.Count > 0) {
                if(dt.Rows[0]["answer_arry"].ToString()==value) {
                    int.TryParse(dt.Rows[0]["choice_score"].ToString(), out score);
                }
            }

            dt = ExecuteQueryData(string.Format(@"SELECT value
                                                            FROM tb_answer_list 
                                                            WHERE title_list_id={0} and user_id={1}", title_list_id, user_id));

            if (dt.Rows.Count > 0) {
                string sqlFormat = @"update  tb_answer_list set value={0},score={1} where  title_list_id={2} and user_id={3}";
                string sql = string.Format(sqlFormat, value, score,title_list_id, user_id);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
            else {
                string sqlFormat = @"insert into tb_answer_list(title_list_id,user_id,value,score)values('{0}','{1}','{2}','{3}')";
                string sql = string.Format(sqlFormat, title_list_id, user_id, value, score);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
        }

        private void InsertAnswerJudge(HttpContext context, string title_list_id, string user_id, string value)
        {
            DataTable dt = ExecuteQueryData(string.Format(@"SELECT p.judge_score,c.answer_arry
                                                FROM tb_title_list as t
                                                INNER JOIN tb_judge as c on t.title_id = c.id
                                                INNER JOIN tb_papers as p on p.id = t.paper_id
                                                WHERE t.type = 'judge' and t.id ={0}", title_list_id));
            int score = 0;
            if (dt.Rows.Count > 0) {
                if (dt.Rows[0]["answer_arry"].ToString() == value) {
                    int.TryParse(dt.Rows[0]["judge_score"].ToString(), out score);
                }
            }
            dt = ExecuteQueryData(string.Format(@"SELECT value
                                                            FROM tb_answer_list 
                                                            WHERE title_list_id={0} and user_id={1}", title_list_id, user_id));
            if (dt.Rows.Count > 0) {
                string sqlFormat = @"update  tb_answer_list set value={0},score={1} where  title_list_id={2} and user_id={3}";
                string sql = string.Format(sqlFormat, value, score, title_list_id, user_id);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
            else {
                string sqlFormat = @"insert into tb_answer_list(title_list_id,user_id,value,score)values({0},{1},{2},{3})";
                string sql = string.Format(sqlFormat, title_list_id, user_id, value, score);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
        }
        
        private void InsertAnswerFilling(HttpContext context, string title_list_id, string user_id, string index,string answer)
        {
            DataTable dt = ExecuteQueryData(string.Format(@"SELECT value
                                                            FROM tb_answer_list 
                                                            WHERE title_list_id={0} and user_id={1}", title_list_id, user_id));
            if (dt.Rows.Count > 0) {
                string oldV = dt.Rows[0]["value"].ToString();
                string []vslues = oldV.Split(',');
                List<string> outV = new List<string>();
                int count = Math.Max(vslues.Length, int.Parse(index)+1);
                for(int i = 0; i < count; i++) {
                    if (i == int.Parse(index)) {
                        outV.Add(answer);
                    }
                    else {
                        if (i < vslues.Length) {
                            outV.Add(vslues[i]);
                        }
                        else {
                            outV.Add("");
                        }
                    }
                }
                answer = string.Join(",",outV.ToArray());

                string sqlFormat = @"update  tb_answer_list set value='{0}' where title_list_id={1} and user_id={2}";
                string sql = string.Format(sqlFormat, answer,  title_list_id, user_id);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
            else {
                string sqlFormat = @"insert into tb_answer_list(title_list_id,user_id,value)values('{0}','{1}','{2}')";
                string sql = string.Format(sqlFormat, title_list_id, user_id, answer);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
        }

        private void InsertAnswerQa(HttpContext context, string title_list_id, string user_id, string value)
        {
            DataTable dt = ExecuteQueryData(string.Format(@"SELECT value
                                                            FROM tb_answer_list 
                                                            WHERE title_list_id={0} and user_id={1}", title_list_id, user_id));
            if (dt.Rows.Count > 0) {
                string sqlFormat = @"update  tb_answer_list set value='{0}' where  title_list_id={1} and user_id={2}";
                string sql = string.Format(sqlFormat, value,  title_list_id, user_id);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0, "答案录入成功");
            }
            else {
                string sqlFormat = @"insert into tb_answer_list(title_list_id,user_id,value)values('{0}','{1}','{2}')";
                string sql = string.Format(sqlFormat, title_list_id, user_id, value);
                int code = ExecuteNoQuery(sql);
                if (code < 0)
                    WriteResponse(context, -1, dbError);
                else
                    WriteResponse(context, 0,"答案录入成功");
            }
        }

        private void UpdateScore(HttpContext context, string title_list_id, string user_id,string score)
        {
            string sqlFormat = @"update tb_answer_list set score={0} where title_list_id={1} and user_id={2}";
            string sql = string.Format(sqlFormat, score,title_list_id, user_id);
            int code = ExecuteNoQuery(sql);

            if (code < 0)
                WriteResponse(context, -1, "评分失败" + dbError);
            else
                WriteResponse(context, 0,"评分成功");
        }
                
        protected void SaveCheckPaper(HttpContext context)
        {
            string user_id= ReadFormStr(context, "userId");
            string paper_id = ReadFormStr(context, "paperId");

            string sqlFormat = @"SELECT sum(score) from tb_title_list as t INNER JOIN tb_answer_list as a on t.id = a.title_list_id
                                                   where t.paper_id = '{0}' and a.user_id = '{1}'";
            string sql = string.Format(sqlFormat, paper_id, user_id);
            DataTable dt = ExecuteQueryData(sql);
            if (dt.Rows.Count < 0) {
                WriteResponse(context, -1, "计算成绩失败!!" + dbError);
            }
            else {
                //先删除
                ExecuteNoQuery(string.Format("delete from tb_check_paper WHERE user_id='{0}' and paper_id='{1}'",user_id,paper_id));

                sqlFormat = @"INSERT INTO tb_check_paper(user_id,paper_id,total_score,check_state,check_name,check_time)
                                                  values('{0}','{1}','{2}','{3}','{4}','{5}') ";
                string state = logonUserType == "1"?"已评分":"未评分";
                string name = logonUserType == "1" ? logonUser : "";
                sql = string.Format(sqlFormat,user_id,paper_id,dt.Rows[0][0].ToString(), state, name, publicFun.GetDateString(DateTime.Now));
                int code = ExecuteNoQuery(sql);
                if(code <0 ) {
                    WriteResponse(context, -1, dbError);
                }
                else {
                    WriteResponse(context, 0, "保存成绩成功");
                }

            }
           
        }

        private void ChangePassword(HttpContext context)
        {
            string user = ReadFormStr(context, "user");
            string old_pass = ReadFormStr(context, "old_pass");
            string new_pass = ReadFormStr(context, "new_pass");
            string new_pass_confirm = ReadFormStr(context, "new_pass_confirm");

            if (user == "" || old_pass == "" || new_pass == "") {
                WriteResponse(context, -1, "输入参数错误");
                return;
            }

            if (new_pass != new_pass_confirm) {
                WriteResponse(context, -1, "密码验证失败");
                return;
            }

            string sqlFormat = "update tb_users set password=\"{0}\" where name=\"{1}\" and password=\"{2}\"";

            string sql = string.Format(sqlFormat, new_pass, user, old_pass);

            int code = ExecuteNoQuery(sql);
            if (code < 0)
                WriteResponse(context, -1, dbError);
            else
                WriteResponse(context, 0);
        }

        protected override void Add(HttpContext context)
        {
            throw new NotImplementedException();
        }

        protected override void Edit(HttpContext context)
        {
            throw new NotImplementedException();
        }

        protected override void Delete(HttpContext context)
        {
            throw new NotImplementedException();
        }

        protected override void Search(HttpContext context)
        {
            throw new NotImplementedException();
        }

        protected override void Default(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}