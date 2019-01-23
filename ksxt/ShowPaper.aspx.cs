using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ksxt
{

    public partial class ShowPaper : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string paper_id = Request.QueryString["paperid"];

            if(paper_id==null || paper_id == "") { return; }

            DataTable dt = dbBase.ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
                return;

            int[] singles =publicFun.StringToNumArry(dt.Rows[0]["choice_id_arry"].ToString());
            int[] fillings = publicFun.StringToNumArry(dt.Rows[0]["filling_id_arry"].ToString());
            int[] judges = publicFun.StringToNumArry(dt.Rows[0]["judge_id_arry"].ToString());
            int[] qas = publicFun.StringToNumArry(dt.Rows[0]["qa_id_arry"].ToString());

            DataTable dt_singles = GetChoices(singles);

            DataTable dt_judges = GetJudges(judges);

            DataTable dt_fillings = GetFillings(fillings);

            DataTable dt_qas = GetQas(qas);

        }

        protected string intArryToStr(int [] arry)
        {
            string sRet = "";

            if (arry.Length == 0)
                return "0";

            foreach(int i in arry)
            {
                sRet += i.ToString();
                sRet += ",";
            }

            sRet = sRet.Remove(sRet.Length - 1);

            return sRet;
        }

        protected DataTable GetChoices(int []ids)
        {
            string sqlFormat = "select * from tb_choice where id in({0})";

            string sql = string.Format(sqlFormat, intArryToStr(ids));

            DataTable dt = dbBase.ExecuteQueryData(sql);            

            return dt;
        }

        protected DataTable GetJudges(int[] ids)
        {
            string sqlFormat = "select * from tb_judge where id in({0})";

            string sql = string.Format(sqlFormat, intArryToStr(ids));

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }

        protected DataTable GetFillings(int[] ids)
        {
            string sqlFormat = "select * from tb_filling where id in({0})";

            string sql = string.Format(sqlFormat, intArryToStr(ids));

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }

        protected DataTable GetQas(int[] ids)
        {
            string sqlFormat = "select * from tb_qa where id in({0})";

            string sql = string.Format(sqlFormat, intArryToStr(ids));

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }
    }
}