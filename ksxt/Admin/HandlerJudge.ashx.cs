using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerJudge 的摘要说明
    /// </summary>
    public class HandlerJudge : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
        }

        protected override void Add(HttpContext context)
        {
            string level = ReadFormStr(context, "f_level");
            string title = ReadFormStr(context, "f_title");
            string answer = ReadFormStr(context, "f_answer");

            if (level == "" || title == "" || answer == "") {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            if (ExecuteQueryDataCount("select * from tb_judge where title='" + title + "'") > 0) {
                WriteResponse(context, -1, "数据重复", "");
                return;
            }

            string sqlFormat = @"insert into tb_judge(level,title,answer_arry,create_name,create_time)values(
                                                        '{0}','{1}','{2}','{3}','{4}')";
            string sql = string.Format(sqlFormat, level, title, answer, logonUser, publicFun.GetDateString(DateTime.Now));

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Edit(HttpContext context)
        {
            string edit_id = ReadFormStr(context, "edit_id");

            string level = ReadFormStr(context, "f_level");
            string title = ReadFormStr(context, "f_title");
            string answer = ReadFormStr(context, "f_answer");

            if (level == "" || title == "") {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            if (level == "" || title == "" || answer == "") {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }
            string sqlFormat = @"update tb_judge set level='{0}',title='{1}',answer_arry='{2}',create_name='{3}',create_time='{4}' where id={5}";

            string sql = string.Format(sqlFormat, level, title, answer, logonUser, publicFun.GetDateString(DateTime.Now), edit_id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Delete(HttpContext context)
        {
            string id = ReadFormStr(context, "delid");

            string sqlFormat = "delete from tb_judge where id={0}";
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
                dt = ExecuteQueryData("select * from tb_judge limit " + rows + " offset " + (page - 1) * rows);
            else
                dt = ExecuteQueryData("select * from tb_judge");

            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_level");
            dtView.Columns.Add("v_title");
            dtView.Columns.Add("v_answer");
            dtView.Columns.Add("v_create_name");
            dtView.Columns.Add("v_create_time");

            foreach (DataRow dr in dt.Rows) {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];
                string lev = dr["level"].ToString();
                if (lev == "1")
                    newDr["v_level"] = "初级";
                else if (lev == "2")
                    newDr["v_level"] = "中级";
                else if (lev == "3")
                    newDr["v_level"] = "高级";
                else
                    newDr["v_level"] = "无";

                newDr["v_title"] = dr["title"];

                string anserSel = dr["answer_arry"].ToString();
                newDr["v_answer"] = anserSel == "0" ? "错误" : "正确";

                newDr["v_create_name"] = dr["create_name"];
                newDr["v_create_time"] = dr["create_time"];

                dtView.Rows.Add(newDr);
            }
            //转JSON
            string dtJson = publicFun.DataTableToJson(dtView);

            string listJson = "\"total\":" + ExecuteQueryDataCount("select * from tb_judge")/*dt.Rows.Count*/ + ",\"rows\":";

            listJson += dtJson;

            WriteResponse(context, 0, "查询成功", listJson);
        }

        void SearchById(HttpContext context)
        {
            string s_id = ReadFormStr(context, "s_id");

            if (s_id == "") {
                WriteResponse(context, -1, "查询失败", "");
                return;
            }
            string title = "";
            string level = "";
            string s_answer = "";

            string responseFormat = "\"title\":\"{0}\",\"level\":\"{1}\",\"answer\":\"{2}\"";

            DataTable dataTable = ExecuteQueryData("select * from tb_judge where id=" + s_id);

            if (dataTable.Rows.Count > 0) {
                title = dataTable.Rows[0]["title"].ToString();
                level = dataTable.Rows[0]["level"].ToString();

                string answers = dataTable.Rows[0]["answer_arry"].ToString();
                s_answer = answers;
            }

            string json = string.Format(responseFormat, title, level, s_answer);

            WriteResponse(context, 0, "操作成功", json);
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