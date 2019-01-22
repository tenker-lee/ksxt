using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerPaper 的摘要说明
    /// </summary>
    public class HandlerPaper : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
        }

        protected override void Add(HttpContext context)
        {
            string title = ReadFormStr(context, "f_title");
            string choice_score = ReadFormStr(context, "f_choice_score");
            string filling_score = ReadFormStr(context, "f_filling_score");
            string judge_score = ReadFormStr(context, "f_judge_score");
            string qa_score = ReadFormStr(context, "f_qa_score");
            string start_time = ReadFormStr(context, "f_start_time");
            string end_time = ReadFormStr(context, "f_end_time");

            if (title == "" || start_time == "" || end_time == "")
            {
                WriteResponse(context, -1, "输入参数有误", "");
                return;
            }

            if (choice_score == "" || filling_score == "" || judge_score == "" || qa_score=="")
            {
                WriteResponse(context, -1, "输入参数有误", "");
                return;
            }
            if (ExecuteQueryDataCount("select * from tb_papers where title='" + title + "'") > 0)
            {
                WriteResponse(context, -1, "数据重复", "");
                return;
            }

            string sqlFormat = @"insert into tb_papers(title,choice_score,filling_score,judge_score,qa_score,start_time,end_time,create_name,create_time)values(
                                                        '{0}',{1},{2},{3},{4},'{5}','{6}','{7}','{8}')";
            string sql = string.Format(sqlFormat, title, 
                                       publicFun.StringToInt(choice_score), publicFun.StringToInt(filling_score), publicFun.StringToInt(judge_score), publicFun.StringToInt(qa_score),
                start_time, end_time, logonUser, publicFun.GetDateString(DateTime.Now));

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Edit(HttpContext context)
        {
            string edit_id = ReadFormStr(context, "edit_id");

            string title = ReadFormStr(context, "f_title");
            string choice_score = ReadFormStr(context, "f_choice_score");
            string filling_score = ReadFormStr(context, "f_filling_score");
            string judge_score = ReadFormStr(context, "f_judge_score");
            string qa_score = ReadFormStr(context, "f_qa_score");
            string start_time = ReadFormStr(context, "f_start_time");
            string end_time = ReadFormStr(context, "f_end_time");

            if (title == "" || start_time == "" || end_time == "")
            {
                WriteResponse(context, -1, "输入参数有误", "");
                return;
            }

            if (choice_score == "" || filling_score == "" || judge_score == "" || qa_score == "")
            {
                WriteResponse(context, -1, "输入参数有误", "");
                return;
            }

            string sqlFormat = @"update tb_papers set title='{0}',choice_score={1},filling_score={2},judge_score={3},qa_score={4},start_time='{5}',end_time='{6}',create_name='{7}',create_time='{8}' where id={9}";

            string sql = string.Format(sqlFormat, title, publicFun.StringToInt(choice_score), publicFun.StringToInt(filling_score), publicFun.StringToInt(judge_score), publicFun.StringToInt(qa_score),
                start_time,end_time, logonUser, publicFun.GetDateString(DateTime.Now), edit_id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Delete(HttpContext context)
        {
            string id = ReadFormStr(context, "delid");

            string sqlFormat = "delete from tb_papers where id={0}";
            string sql = string.Format(sqlFormat, id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Search(HttpContext context)
        {
            int page = publicFun.StringToInt(ReadFormStr(context, "page"));
            int rows = publicFun.StringToInt(ReadFormStr(context, "rows"));

            DataTable dt;
            if (page > 0 && rows > 0)
                dt = ExecuteQueryData("select * from tb_papers limit " + rows + " offset " + (page - 1) * rows);
            else
                dt = ExecuteQueryData("select * from tb_papers");

            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_title");
            dtView.Columns.Add("v_choice_id_arry");
            dtView.Columns.Add("v_choice_score");
            dtView.Columns.Add("v_filling_id_arry");
            dtView.Columns.Add("v_filling_score");
            dtView.Columns.Add("v_judge_id_arry");
            dtView.Columns.Add("v_judge_score");
            dtView.Columns.Add("v_qa_id_arry");
            dtView.Columns.Add("v_qa_score");
            dtView.Columns.Add("v_start_time");
            dtView.Columns.Add("v_end_time");
            dtView.Columns.Add("v_create_name");
            dtView.Columns.Add("v_create_time");

            foreach (DataRow dr in dt.Rows)
            {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];           

                newDr["v_title"] = dr["title"];
                newDr["v_choice_id_arry"] = dr["choice_id_arry"];
                newDr["v_choice_score"] = dr["choice_score"];
                newDr["v_filling_id_arry"] = dr["filling_id_arry"];
                newDr["v_filling_score"] = dr["filling_score"];
                newDr["v_judge_id_arry"] = dr["judge_id_arry"];
                newDr["v_judge_score"] = dr["judge_score"];
                newDr["v_qa_id_arry"] = dr["qa_id_arry"];
                newDr["v_qa_score"] = dr["qa_score"];
                newDr["v_start_time"] = dr["start_time"];
                newDr["v_end_time"] = dr["end_time"];

                newDr["v_create_name"] = dr["create_name"];
                newDr["v_create_time"] = dr["create_time"];

                dtView.Rows.Add(newDr);
            }
            //转JSON
            string dtJson = publicFun.DataTableToJson(dtView);

            string listJson = "\"total\":" + ExecuteQueryDataCount("select * from tb_papers") + ",\"rows\":";

            listJson += dtJson;

            WriteResponse(context, 0, "查询成功", listJson);
        }

        void SearchById(HttpContext context)
        {
            string s_id = ReadFormStr(context, "s_id");

            if (s_id == "")
            {
                WriteResponse(context, -1, "查询失败", "");
                return;
            }
            string title = "";
            string choice_score="";
            string filling_score = "";
            string judge_score = "";
            string qa_score = "";
            string start_time = "";
            string end_time = "";

            string responseFormat = "\"title\":\"{0}\",\"choice_score\":{1},\"filling_score\":{2},\"judge_score\":{3},\"qa_score\":{4},\"start_time\":\"{5}\",\"end_time\":\"{6}\"";

            DataTable dataTable = ExecuteQueryData("select * from tb_papers where id=" + s_id);

            if (dataTable.Rows.Count > 0)
            {
                title = dataTable.Rows[0]["title"].ToString();
                choice_score = dataTable.Rows[0]["choice_score"].ToString();
                filling_score = dataTable.Rows[0]["filling_score"].ToString();
                judge_score = dataTable.Rows[0]["judge_score"].ToString();
                qa_score = dataTable.Rows[0]["qa_score"].ToString();
                start_time = dataTable.Rows[0]["start_time"].ToString();
                end_time = dataTable.Rows[0]["end_time"].ToString();
            }

            string json = string.Format(responseFormat, title, choice_score, filling_score, judge_score, qa_score,start_time, end_time);

            WriteResponse(context, 0, "操作成功", json);
        }

        void AddChoiceToPaper(HttpContext context) {

            string optType = ReadFormStr(context, "optType");
            string paper_id = ReadFormStr(context, "paper_id");
            string title_id = ReadFormStr(context, "title_id");

            if(optType=="" || paper_id=="" || title_id == "")
            {
                WriteResponse(context, -1, "参数错误");
                return;
            }

            DataTable dt = ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
            {
                WriteResponse(context, -1, "无此试卷");
                return;
            }

            string[] choice = publicFun.StringToArry(dt.Rows[0]["choice_id_arry"].ToString());

            List<string> list = choice.ToList<string>();

            if (!choice.Contains(title_id))
            {
                if (optType == "add")
                {
                    list.Add(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set choice_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }
            else
            {
                if (optType == "del")
                {
                    list.Remove(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set choice_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }

            WriteResponse(context, 0,"操作成功","\"title_list\":\""+ publicFun.ArryToString(list.ToArray()) + "\"");
        }

        void AddFillingToPaper(HttpContext context) {

            string optType = ReadFormStr(context, "optType");
            string paper_id = ReadFormStr(context, "paper_id");
            string title_id = ReadFormStr(context, "title_id");

            if (optType == "" || paper_id == "" || title_id == "")
            {
                WriteResponse(context, -1, "参数错误");
                return;
            }

            DataTable dt = ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
            {
                WriteResponse(context, -1, "无此试卷");
                return;
            }

            string[] choice = publicFun.StringToArry(dt.Rows[0]["filling_id_arry"].ToString());

            List<string> list = choice.ToList<string>();

            if (!choice.Contains(title_id))
            {
                if (optType == "add")
                {
                    list.Add(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set filling_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }
            else
            {
                if (optType == "del")
                {
                    list.Remove(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set filling_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }

            WriteResponse(context, 0, "操作成功", "\"title_list\":\"" + publicFun.ArryToString(list.ToArray()) + "\"");
        }

        void AddJudgeToPaper(HttpContext context) {

            string optType = ReadFormStr(context, "optType");
            string paper_id = ReadFormStr(context, "paper_id");
            string title_id = ReadFormStr(context, "title_id");

            if (optType == "" || paper_id == "" || title_id == "")
            {
                WriteResponse(context, -1, "参数错误");
                return;
            }

            DataTable dt = ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
            {
                WriteResponse(context, -1, "无此试卷");
                return;
            }

            string[] choice = publicFun.StringToArry(dt.Rows[0]["judge_id_arry"].ToString());

            List<string> list = choice.ToList<string>();

            if (!choice.Contains(title_id))
            {
                if (optType == "add")
                {
                    list.Add(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set judge_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }
            else
            {
                if (optType == "del")
                {
                    list.Remove(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set judge_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }

            WriteResponse(context, 0, "操作成功", "\"title_list\":\"" + publicFun.ArryToString(list.ToArray()) + "\"");
        }

        void AddQaToPaper(HttpContext context) {

            string optType = ReadFormStr(context, "optType");
            string paper_id = ReadFormStr(context, "paper_id");
            string title_id = ReadFormStr(context, "title_id");

            if (optType == "" || paper_id == "" || title_id == "")
            {
                WriteResponse(context, -1, "参数错误");
                return;
            }

            DataTable dt = ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
            {
                WriteResponse(context, -1, "无此试卷");
                return;
            }

            string[] choice = publicFun.StringToArry(dt.Rows[0]["qa_id_arry"].ToString());

            List<string> list = choice.ToList<string>();

            if (!choice.Contains(title_id))
            {
                if (optType == "add")
                {
                    list.Add(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set qa_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }
            else
            {
                if (optType == "del")
                {
                    list.Remove(title_id);
                    list.Sort();
                    int code = ExecuteNoQuery("update tb_papers set qa_id_arry='" + publicFun.ArryToString(list.ToArray()) + "' where id=" + paper_id);
                    if (code < 0)
                    {
                        WriteResponse(context, -2, dbError);
                        return;
                    }
                }
            }

            WriteResponse(context, 0, "操作成功", "\"title_list\":\"" + publicFun.ArryToString(list.ToArray()) + "\"");
        }

        protected override void Default(HttpContext context)
        {
            WriteResponse(context, 0, "hello", "\"total\":0,\"rows\":[]");
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