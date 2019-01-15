using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerUser 的摘要说明
    /// </summary>
    public class HandlerUser : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
        }

        protected override void Add(HttpContext context)
        {
            string type = ReadFormStr(context, "f_type");
            string name = ReadFormStr(context, "f_name");
            string password = ReadFormStr(context, "f_password");
            string department = ReadFormStr(context, "f_department");
            string job = ReadFormStr(context, "f_job");

            if (type == "" || name == "" || password == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            string sqlFormat = @"insert into tb_users(type,name,password,department,job_title,create_name,create_time)values(
                                                        '{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            string sql = string.Format(sqlFormat, type, name, password, department, job,logonUser, publicFun.GetDateString(DateTime.Now));

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Edit(HttpContext context)
        {
            string edit_id = ReadFormStr(context, "edit_id");

            string type = ReadFormStr(context, "f_type");
            string name = ReadFormStr(context, "f_name");
            string password = ReadFormStr(context, "f_password");
            string department = ReadFormStr(context, "f_department");
            string job = ReadFormStr(context, "f_job");

            if (type == "" || name == "" || password == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            string sqlFormat = @"update tb_users set type='{0}',name='{1}',password='{2}',department='{3}',job_title='{4}',create_name='{5}',create_time='{6}' where id={7}";

            string sql = string.Format(sqlFormat, type, name, password, department, job, logonUser, publicFun.GetDateString(DateTime.Now), edit_id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Delete(HttpContext context)
        {
            string id = ReadFormStr(context, "delid");

            string sqlFormat = "delete from tb_users where id={0}";
            string sql = string.Format(sqlFormat, id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        protected override void Search(HttpContext context)
        {
            DataTable dt = ExecuteQueryData("select * from tb_users");

            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_type");
            dtView.Columns.Add("v_name");
            dtView.Columns.Add("v_password");
            dtView.Columns.Add("v_department");
            dtView.Columns.Add("v_job");
            dtView.Columns.Add("v_create_name");
            dtView.Columns.Add("v_create_time");

            foreach (DataRow dr in dt.Rows)
            {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];          
                newDr["v_type"] = dr["type"];
                newDr["v_name"] = dr["name"];
                newDr["v_password"] = dr["password"];
                newDr["v_department"] = dr["department"];
                newDr["v_job"] = dr["job_title"];
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