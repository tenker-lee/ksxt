using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;

namespace ksxt
{
    /// <summary>
    /// HandlerPublicFun 的摘要说明
    /// </summary>
    public class HandlerPublicFun : dbBase, IHttpHandler, IRequiresSessionState
    {
        private string user_id = "";

        protected string ReadFormStr(HttpContext context, string itemName)
        {
            if (context.Request.Form[itemName] == null)
                return "";
            else
                return context.Request.Form[itemName];
        }

        public void ProcessRequest(HttpContext context)
        {
            user_id = context.Session["logonId"].ToString();

            string opt = context.Request.QueryString["opt"];
            if (opt == null)
                opt = "";

            Type thisType = this.GetType();
            MethodInfo method = thisType.GetMethod(opt, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
                WriteResponse(context, 0, "hello", "");
            else
                method.Invoke(this, new object[] { context });
        }

        private void GetDateTime(HttpContext context)
        {
            string dateStr = publicFun.GetDateString(DateTime.Now);
            WriteResponse(context, 0, "查询成功", "\"now\":\"" + dateStr + "\"");
        }

        private void WriteResponse(HttpContext context, int stateCode, string msg = "操作成功", string usrStr = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            context.Response.Clear();

            stringBuilder.Append("{");
            stringBuilder.Append("\"stateCode\":");
            stringBuilder.Append(stateCode.ToString());
            stringBuilder.Append(",");

            stringBuilder.Append("\"msg\":\"");
            stringBuilder.Append(msg);
            stringBuilder.Append("\"");

            if (usrStr != null && usrStr != "") {
                stringBuilder.Append(",");
                stringBuilder.Append(usrStr);
            }

            stringBuilder.Append("}");

            context.Response.Write(stringBuilder.ToString());
        }

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
            string answerStr = ReadFormStr(context, "answerStr");
            string answer = ReadFormStr(context, "value");

            string type="";
            string  titleid="";
            string value ="";

            MatchCollection matchCollection = Regex.Matches(answerStr, "^(?<type>\\S+)_(?<titleid>\\d+)_answer_(?<value>\\d+)$");
            if (matchCollection.Count > 0) {
                type = matchCollection[0].Groups["type"].Value;
                titleid = matchCollection[0].Groups["titleid"].Value;
                value = matchCollection[0].Groups["value"].Value;
            }                      
           
            string sqlFormat,sql;
            int code;

            if (user_id != "") {

                sqlFormat = "delete from tb_answer_list where user_id={0} and title_list_id={1} ";
                sql = string.Format(sqlFormat, user_id, titleid);

                code = ExecuteNoQuery(sql);
                if (code < 0) {
                    WriteResponse(context, -1, dbError);
                    return;
                }
            }
            // 插入答案
            sqlFormat = @"insert into tb_answer_list(title_list_id,user_id,value)values({0},{1},{2})";
            sql = string.Format(sqlFormat, titleid, user_id, value);
            code = ExecuteNoQuery(sql);
            if (code < 0)
                WriteResponse(context, -1, dbError);
            else
                WriteResponse(context, 0);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}