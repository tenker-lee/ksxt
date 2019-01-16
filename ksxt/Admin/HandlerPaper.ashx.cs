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
            string start_time = ReadFormStr(context, "f_start_time");
            string end_time = ReadFormStr(context, "f_end_time");

            if (title == "" || start_time == "" || end_time == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            string sqlFormat = @"insert into tb_papers(title,start_time,end_time,create_name,create_time)values(
                                                        '{0}','{1}','{2}','{3}','{4}')";
            string sql = string.Format(sqlFormat, title, start_time, end_time, logonUser, publicFun.GetDateString(DateTime.Now));

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
            string start_time = ReadFormStr(context, "f_start_time");
            string end_time = ReadFormStr(context, "f_end_time");

            if (title == "" || start_time == "" || end_time == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }
            string sqlFormat = @"update tb_papers set title='{0}',start_time='{1}',end_time='{2}',create_name='{3}',create_time='{4}' where id={5}";

            string sql = string.Format(sqlFormat, title, start_time,end_time, logonUser, publicFun.GetDateString(DateTime.Now), edit_id);

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
            DataTable dt = ExecuteQueryData("select * from tb_papers");
            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_title");
            dtView.Columns.Add("v_start_time");
            dtView.Columns.Add("v_end_time");
            dtView.Columns.Add("v_create_name");
            dtView.Columns.Add("v_create_time");

            foreach (DataRow dr in dt.Rows)
            {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];           

                newDr["v_title"] = dr["title"];
                newDr["v_start_time"] = dr["start_time"];
                newDr["v_end_time"] = dr["end_time"];

                newDr["v_create_name"] = dr["create_name"];
                newDr["v_create_time"] = dr["create_time"];

                dtView.Rows.Add(newDr);
            }
            //转JSON
            string dtJson = publicFun.DataTableToJson(dtView);

            string listJson = "\"total\":" + dt.Rows.Count + ",\"rows\":";

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
            string start_time = "";
            string end_time = "";

            string responseFormat = "\"title\":\"{0}\",\"start_time\":\"{1}\",\"end_time\":\"{2}\"";

            DataTable dataTable = ExecuteQueryData("select * from tb_papers where id=" + s_id);

            if (dataTable.Rows.Count > 0)
            {
                title = dataTable.Rows[0]["title"].ToString();
                start_time = dataTable.Rows[0]["start_time"].ToString();
                end_time = dataTable.Rows[0]["end_time"].ToString();
            }

            string json = string.Format(responseFormat, title, start_time, end_time);

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