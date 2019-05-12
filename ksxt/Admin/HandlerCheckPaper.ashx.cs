using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerCheckPaper 的摘要说明
    /// </summary>
    public class HandlerCheckPaper : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
        }

        protected override void Add(HttpContext context)
        {
            //throw new NotImplementedException();
        }

        protected override void Edit(HttpContext context)
        {
            //throw new NotImplementedException();
        }

        protected override void Delete(HttpContext context)
        {
            //throw new NotImplementedException();
        }

        protected override void Search(HttpContext context)
        {
            int page = publicFun.StringToInt(ReadFormStr(context, "page"));
            int rows = publicFun.StringToInt(ReadFormStr(context, "rows"));

            string papername = ReadFormStr(context, "name");
            string paperid = ReadFormStr(context, "paperid");

            string search = " 1=1 ";

            if (paperid != "") {
                search += " and p.id='" + paperid + "' ";
            }
            if (papername != "") {
                search += " and u.name like '%" + papername + "%'";
            }

            if (string.IsNullOrEmpty(logonId)) {
                WriteResponse(context, 0, "登录超时请重新登录1!", "");
                return;
            }
            else {
                if(logonUserType!="1")
                    search="  ch.user_id=\""+ logonId + "\"";
            }

            DataTable dt;
            if (page > 0 && rows > 0)
                dt = ExecuteQueryData(@"select ch.*,p.title,p.end_time, u.name,u.id as uid,u.type
                                        from 
                                        tb_check_paper as ch INNER join tb_papers as p on p.id=ch.paper_id 
                                        left join tb_users as u on ch.user_id=u.id where " + search+" order by title limit " + rows + " offset " + (page - 1) * rows);
            else
                dt = ExecuteQueryData(@"select ch.*,p.title,p.end_time, u.name ,uid as uid 
                                        from tb_check_paper as ch INNER join tb_papers as p on p.id=ch.paper_id 
                                        left join tb_users as u on ch.user_id=u.id where " + search + " order by title");

            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_paper_id");
            dtView.Columns.Add("v_title");
            dtView.Columns.Add("v_uid");
            dtView.Columns.Add("v_utype");
            dtView.Columns.Add("v_user_name");
            dtView.Columns.Add("v_total_score");
            dtView.Columns.Add("v_check_state");
            dtView.Columns.Add("v_check_name");
            dtView.Columns.Add("v_check_time");
            dtView.Columns.Add("v_paper_end");           

            foreach (DataRow dr in dt.Rows) {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];
                newDr["v_paper_id"] = dr["paper_id"];
                newDr["v_title"] = dr["title"];
                newDr["v_uid"] = dr["uid"];
                newDr["v_utype"] = logonUserType ;
                newDr["v_user_name"] = dr["name"];
                newDr["v_total_score"] =dr["total_score"] ;
                newDr["v_check_state"] = dr["check_state"];
                newDr["v_check_name"] = dr["check_name"];
                newDr["v_check_time"] = dr["check_time"];
                DateTime end = publicFun.GetDateTimeFromStr(dr["end_time"].ToString());
                newDr["v_paper_end"] = DateTime.Now > end ? "1" : "0";
                dtView.Rows.Add(newDr);
            }
            //转JSON
            string dtJson = publicFun.DataTableToJson(dtView);

            string listJson = "\"total\":" + ExecuteQueryDataCount(@"select ch.*,p.title, u.name 
                                                                     from tb_check_paper as ch INNER join tb_papers as p on p.id=ch.paper_id 
                                                                     left join tb_users as u on ch.user_id=u.id") + ",\"rows\":";

            listJson += dtJson;

            WriteResponse(context, 0, "查询成功", listJson);
        }

        protected override void Default(HttpContext context)
        {
            //throw new NotImplementedException();
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