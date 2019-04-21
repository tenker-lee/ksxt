using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerMyPaper 的摘要说明
    /// </summary>
    public class HandlerMyPaper : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
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
            int page = publicFun.StringToInt(ReadFormStr(context, "page"));
            int rows = publicFun.StringToInt(ReadFormStr(context, "rows"));

            DataTable dt;
            if (page > 0 && rows > 0) {
                dt = ExecuteQueryData("select * from tb_papers limit " + rows + " offset " + (page - 1) * rows);
            }
            else {
                dt = ExecuteQueryData("select * from tb_papers");
            }
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

            foreach (DataRow dr in dt.Rows) {
                DataRow newDr = dtView.NewRow();
                newDr["v_id"] = dr["id"];
                newDr["v_title"] = dr["title"];
                newDr["v_choice_id_arry"] = publicFun.dtToids(ExecuteQueryData(string.Format("select * from tb_title_list where paper_id='{0}' and type='choice' order by title_id", dr["id"])), "title_id");
                newDr["v_choice_score"] = dr["choice_score"];
                newDr["v_filling_id_arry"] = publicFun.dtToids(ExecuteQueryData(string.Format("select * from tb_title_list where paper_id='{0}' and type='filling' order by title_id", dr["id"])), "title_id");
                newDr["v_filling_score"] = dr["filling_score"];
                newDr["v_judge_id_arry"] = publicFun.dtToids(ExecuteQueryData(string.Format("select * from tb_title_list where paper_id='{0}' and type='judge' order by title_id", dr["id"])), "title_id");
                newDr["v_judge_score"] = dr["judge_score"];
                newDr["v_qa_id_arry"] = publicFun.dtToids(ExecuteQueryData(string.Format("select * from tb_title_list where paper_id='{0}' and type='qa' order by title_id", dr["id"])), "title_id");
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