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
            string password_confirm = ReadFormStr(context, "f_password_confirm");
            
            string department = ReadFormStr(context, "f_department");
            string job = ReadFormStr(context, "f_job");

            if (type == "" || name == "" || password == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            if(password != password_confirm)
            {
                WriteResponse(context, -1, "两次输入密码不一致", "");
                return;
            }

            if (ExecuteQueryDataCount("select * from tb_users where name='" + name + "'") > 0)
            {
                WriteResponse(context, -1, "数据重复", "");
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
                WriteResponse(context, -1, "输入参数有误", "");
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
            int page = publicFun.StringToInt(ReadFormStr(context, "page"));
            int rows = publicFun.StringToInt(ReadFormStr(context, "rows"));
            DataTable dt;
            if (page > 0 && rows > 0)
                dt = ExecuteQueryData("select * from tb_users limit " + rows + " offset " + (page - 1) * rows);
            else
                dt = ExecuteQueryData("select * from tb_users");
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
                newDr["v_type"] = dr["type"].ToString()=="1"? "管理员" : "普通";
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

            string listJson = "\"total\":" + ExecuteQueryDataCount("select * from tb_users") + ",\"rows\":";

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
            string type = "";
            string name = "";
            string department = "";
            string job = "";

            string responseFormat = "\"type\":\"{0}\",\"name\":\"{1}\",\"department\":\"{2}\",\"job\":\"{3}\"";

            DataTable dataTable = ExecuteQueryData("select * from tb_users where id=" + s_id);

            if (dataTable.Rows.Count > 0)
            {
                type = dataTable.Rows[0]["type"].ToString();
                name = dataTable.Rows[0]["name"].ToString();
                department = dataTable.Rows[0]["department"].ToString();
                job = dataTable.Rows[0]["job_title"].ToString();
            }

            string json = string.Format(responseFormat, type, name, department,job);

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