using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ksxt
{

    public partial class ShowPaper : CustomPage/*System.Web.UI.Page*/
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string paper_id = Request.QueryString["paperid"];

            if(paper_id==null || paper_id == "") { return; }

            DataTable dt = dbBase.ExecuteQueryData("select * from tb_papers where id=" + paper_id);                   
            
            if (dt.Rows.Count < 1)
                return;

            int choice_score = publicFun.StringToInt(dt.Rows[0]["choice_score"].ToString());
            int judge_score = publicFun.StringToInt(dt.Rows[0]["judge_score"].ToString());
            int filling_score = publicFun.StringToInt(dt.Rows[0]["filling_score"].ToString());
            int qa_score = publicFun.StringToInt(dt.Rows[0]["qa_score"].ToString());

            lab_choice_score.Text = choice_score.ToString();
            lab_judge_score.Text = judge_score.ToString();
            lab_filling_score.Text = filling_score.ToString();
            lab_qa_score.Text = qa_score.ToString();

            int[] singles =publicFun.StringToNumArry(dt.Rows[0]["choice_id_arry"].ToString());
            int[] fillings = publicFun.StringToNumArry(dt.Rows[0]["filling_id_arry"].ToString());
            int[] judges = publicFun.StringToNumArry(dt.Rows[0]["judge_id_arry"].ToString());
            int[] qas = publicFun.StringToNumArry(dt.Rows[0]["qa_id_arry"].ToString());

            lab_qa_count.Text = qas.Length.ToString();
            lab_judge_count.Text = judges.Length.ToString();
            lab_filling_count.Text = fillings.Length.ToString();
            lab_choice_count.Text = singles.Length.ToString();

            lab_choice_total.Text = (choice_score * singles.Length).ToString();
            lab_judge_total.Text = (judge_score * judges.Length).ToString();
            lab_filling_total.Text = (filling_score * fillings.Length).ToString(); 
            lab_qa_total.Text = (qa_score * qas.Length).ToString();

            lab_total_score_paper.Text = (choice_score * singles.Length + judge_score * judges.Length 
                + filling_score * fillings.Length + qa_score * qas.Length).ToString();

            labTime.Text = dt.Rows[0]["start_time"].ToString() + "-" + dt.Rows[0]["end_time"].ToString();

            lab_user.Text = logonUser;

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