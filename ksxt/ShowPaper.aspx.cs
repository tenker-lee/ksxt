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
        protected string paper_id=string.Empty;

        protected string user_id=string.Empty;

        protected bool enableEdit = true;

        protected void Page_Load(object sender, EventArgs e)
        {

            paper_id = Request.QueryString["paperid"];
            paper_id = paper_id == null ? "":paper_id;

            user_id = Request.QueryString["userid"];
            user_id = user_id == null ? "" : user_id;

            if (paper_id == null || paper_id == "") {
                return;
            }

            DataTable us = dbBase.ExecuteQueryData("select * from tb_users where id=" + user_id);
            if (us.Rows.Count > 0) {
                lab_user.Text = us.Rows[0]["name"].ToString();
            }

            DataTable dt = dbBase.ExecuteQueryData("select * from tb_papers where id=" + paper_id);

            if (dt.Rows.Count < 1)
                return;

            int choice_score = publicFun.StringToInt(dt.Rows[0]["choice_score"].ToString());
            int judge_score = publicFun.StringToInt(dt.Rows[0]["judge_score"].ToString());
            int filling_score = publicFun.StringToInt(dt.Rows[0]["filling_score"].ToString());
            int qa_score = publicFun.StringToInt(dt.Rows[0]["qa_score"].ToString());

            lab_title.Text = "试卷名:" + dt.Rows[0]["title"].ToString();

            lab_choice_score.Text = choice_score.ToString();
            lab_judge_score.Text = judge_score.ToString();
            lab_filling_score.Text = filling_score.ToString();
            lab_qa_score.Text = qa_score.ToString();

            string sqlChoice = string.Format(@"SELECT t.id as tid,t.paper_id,t.type,c.id,c.title,c.select_arry,c.answer_arry,a.user_id,a.value,a.score 
                                                FROM tb_title_list as t 
                                                INNER JOIN tb_choice as c on t.title_id=c.id
                                                LEFT JOIN tb_answer_list as a on a.title_list_id=t.id
                                                WHERE t.type='choice' and t.paper_id='{0}'", paper_id);

            string sqlFilling = string.Format(@"SELECT t.id as tid,t.paper_id,t.type,f.id,f.title,f.answer_arry,a.user_id,a.value,a.score 
                                                FROM tb_title_list as t 
                                                INNER JOIN tb_filling as f on t.title_id=f.id
                                                LEFT JOIN tb_answer_list as a on a.title_list_id=t.id
                                                WHERE t.type='filling' and t.paper_id='{0}'", paper_id);

            string sqlJudge= string.Format(@"SELECT t.id as tid,t.paper_id,t.type,j.id,j.title,j.answer_arry,a.user_id,a.value,a.score 
                                             FROM tb_title_list as t 
                                             INNER JOIN tb_judge as j on t.title_id=j.id
                                             LEFT JOIN tb_answer_list as a on a.title_list_id=t.id
                                             WHERE t.type='judge' and t.paper_id='{0}'", paper_id);

            string sqlQa= string.Format(@"SELECT t.id as tid,t.paper_id,t.type,q.id,q.title,q.answer,a.user_id,a.value,a.score 
                                            FROM tb_title_list as t 
                                            INNER JOIN tb_qa as q on t.title_id=q.id
                                            LEFT JOIN tb_answer_list as a on a.title_list_id=t.id
                                            WHERE t.type='qa' and t.paper_id='{0}'", paper_id);

            DataTable dtSingles = dbBase.ExecuteQueryData(sqlChoice);
            DataTable dtFillings = dbBase.ExecuteQueryData(sqlFilling);
            DataTable dtJudges = dbBase.ExecuteQueryData(sqlJudge);
            DataTable dtQas = dbBase.ExecuteQueryData(sqlQa);
            
            lab_qa_count.Text = dtQas.Rows.Count.ToString();
            lab_judge_count.Text = dtJudges.Rows.Count.ToString();
            lab_filling_count.Text = dtFillings.Rows.Count.ToString();
            lab_choice_count.Text = dtSingles.Rows.Count.ToString();

            lab_choice_total.Text = (choice_score * dtSingles.Rows.Count).ToString();
            lab_judge_total.Text = (judge_score * dtJudges.Rows.Count).ToString();
            lab_filling_total.Text = (filling_score * dtFillings.Rows.Count).ToString();
            lab_qa_total.Text = (qa_score * dtQas.Rows.Count).ToString();

            lab_total_score_paper.Text = (choice_score * dtSingles.Rows.Count + judge_score * dtJudges.Rows.Count
                + filling_score * dtFillings.Rows.Count + qa_score * dtQas.Rows.Count).ToString();

            labTime.Text = dt.Rows[0]["start_time"].ToString() + "-" + dt.Rows[0]["end_time"].ToString();

            //DataTable dt_singles = GetChoices(singles);

            repChoice.DataSource = dtSingles;
            repChoice.DataBind();

            //DataTable dt_judges = GetJudges(judges);

            repJudge.DataSource = dtJudges;
            repJudge.DataBind();

            //DataTable dt_fillings = GetFillings(fillings);
            repFilling.DataSource = dtFillings;
            repFilling.DataBind();

            //DataTable dt_qas = GetQas(qas);
            repQa.DataSource = dtQas;
            repQa.DataBind();

        }

        protected string choiceAnswerTochar(string answer)
        {
            if (answer == "0")
                return "A";
            else if (answer == "1")
                return "B";
            else if (answer == "2")
                return "C";
            else if (answer == "3")
                return "D";
            else
                return "";
        }

        protected string fillingHtml(int id,string strArry)
        {
            string [] arry = publicFun.StringToArry(strArry);
            string htmlStr = ""; 
            for (int i = 0; i < arry.Length; i++)
            {
                htmlStr += "<input class=\"easyui-textbox\" style=\"margin: 2px 2px 2px 2px;width:80px\" id=\"filling_" + id+"_answer_"+i+ "\" name=\"_answer_2\" type=\"text\"/>&nbsp";
            }
            
            return htmlStr;
        }

        protected string[] strToArry(string strArry)
        {
            return publicFun.StringToArry(strArry);
        }

        protected string ReadArryString(string strArry,int index)
        {
            return ReadArryString(publicFun.StringToArry(strArry), index);
        }

        protected string ReadArryString(string[] arry,int index)
        {
            if (index > arry.Length - 1)
                return "";
            else
                return arry[index];
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