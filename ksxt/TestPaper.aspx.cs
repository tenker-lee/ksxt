using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ksxt
{

    public partial class TestPaper : CustomPage
    {
        protected string paper_id=string.Empty;

        protected string user_id=string.Empty;

        protected bool enableEdit = true;

        protected bool showAnswer = true;

        protected bool grade = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            //showAnswer = false;
            //enableEdit = false;
            //grade = false;           
          
            if (!string.IsNullOrEmpty(Request.QueryString["opt"])) {
                if (Request.QueryString["opt"] == "view") {
                    grade = false;
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["paperid"])) {
                paper_id = Request.QueryString["paperid"];
            }
            try {
                user_id = Session["logonId"].ToString();
            }
            catch {

            }
            if(!string.IsNullOrEmpty(Request.QueryString["userid"])) {
                user_id = Request.QueryString["userid"];
            }             

            //标题
            lab_title.Text = "试卷名:随机练习" ;                   

            string sqlChoice = string.Format(@"SELECT t.id as tid,t.paper_id,t.type,c.id,c.title,c.select_arry,c.answer_arry 
                                               FROM tb_title_list as t 
                                               INNER JOIN tb_choice as c on t.title_id=c.id                                              
                                               WHERE t.type='choice' and t.paper_id='{0}'", paper_id);

            string sqlFilling = string.Format(@"SELECT t.id as tid,t.paper_id,t.type,f.id,f.title,f.answer_arry 
                                                FROM tb_title_list as t 
                                                INNER JOIN tb_filling as f on t.title_id=f.id                                               
                                                WHERE t.type='filling' and t.paper_id='{0}'", paper_id);

            string sqlJudge= string.Format(@"SELECT t.id as tid,t.paper_id,t.type,j.id,j.title,j.answer_arry
                                             FROM tb_title_list as t 
                                             INNER JOIN tb_judge as j on t.title_id=j.id                                           
                                             WHERE t.type='judge' and t.paper_id='{0}'",paper_id);

            string sqlQa= string.Format(@"SELECT t.id as tid,t.paper_id,t.type,q.id,q.title,q.answer
                                          FROM tb_title_list as t 
                                          INNER JOIN tb_qa as q on t.title_id=q.id                                         
                                          WHERE t.type='qa' and t.paper_id='{0}'", paper_id);

            DataTable dtSingles = GetChoices();//dbBase.ExecuteQueryData(sqlChoice);
            DataTable dtFillings = GetFillings();//dbBase.ExecuteQueryData(sqlFilling);
            DataTable dtJudges = GetJudges(); //dbBase.ExecuteQueryData(sqlJudge);
            DataTable dtQas = GetQas();//dbBase.ExecuteQueryData(sqlQa);
                      
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

        protected string fillingHtml(int id,string strArry,string value)
        {
            string [] arry = publicFun.StringToArry(strArry);
            string[] answers = value.Split(',');

            string disb = enableEdit ? "" : " disabled =\"disabled\"";

            string htmlStr = "";
            string s = "";
            for (int i = 0; i < arry.Length; i++)
            {
                s = "";
                if (i < answers.Length) {
                    s = answers[i];
                }
                htmlStr += "<input class=\"easyui-textbox\" style=\"margin: 2px 2px 2px 2px;width:80px\" id=\"filling_" + id +"_answer_"+i+ "\"" + disb +" name=\"_answer_2\" type=\"text\" value=\" "+ s+"\"/>&nbsp";
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

        protected DataTable GetChoices()
        {
            string sqlFormat = "select * from tb_choice order by random() LIMIT 5";

            string sql = string.Format(sqlFormat);

            DataTable dt = dbBase.ExecuteQueryData(sql);            

            return dt;
        }

        protected DataTable GetJudges()
        {
            string sqlFormat = "SELECT * from tb_judge order by random() LIMIT 5";

            string sql = string.Format(sqlFormat);

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }

        protected DataTable GetFillings()
        {
            string sqlFormat = "select * from tb_filling order by random() LIMIT 5";

            string sql = string.Format(sqlFormat);

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }

        protected DataTable GetQas()
        {
            string sqlFormat = "select * from tb_qa order by random() LIMIT 2";

            string sql = string.Format(sqlFormat);

            DataTable dt = dbBase.ExecuteQueryData(sql);

            return dt;
        }

        
    }
}