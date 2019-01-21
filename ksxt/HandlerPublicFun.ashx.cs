using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Threading;

namespace ksxt
{
    /// <summary>
    /// HandlerPublicFun 的摘要说明
    /// </summary>
    public class HandlerPublicFun : dbBase,IHttpHandler, IRequiresSessionState
    {
        protected string ReadFormStr(HttpContext context, string itemName)
        {
            if (context.Request.Form[itemName] == null)
                return "";
            else
                return context.Request.Form[itemName];
        }

        public void ProcessRequest(HttpContext context)
        {
            string opt = context.Request.QueryString["opt"];
            if (opt == null)
                opt = "";          
         
            Type thisType = this.GetType();
            MethodInfo method = thisType.GetMethod(opt, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
                WriteResponse(context, 0, "hello", "");
            else
                method.Invoke(this, new object[] { context });            
        }

        private void GetDateTime(HttpContext context)
        {
            string dateStr = publicFun.GetDateString(DateTime.Now);
            WriteResponse(context, 0, "查询成功", "\"now\":\""+dateStr+"\"");
        }

        private void WriteResponse(HttpContext context, int stateCode, string msg="操作成功", string usrStr="")
        {
            StringBuilder stringBuilder = new StringBuilder();
            context.Response.Clear();

            stringBuilder.Append("{");
            stringBuilder.Append("\"stateCode\":");
            stringBuilder.Append(stateCode.ToString());
            stringBuilder.Append(",");

            stringBuilder.Append("\"msg\":\"");
            stringBuilder.Append(msg);
            stringBuilder.Append("\"");

            if (usrStr != null && usrStr != "")
            {
                stringBuilder.Append(",");
                stringBuilder.Append(usrStr);
            }

            stringBuilder.Append("}");

            context.Response.Write(stringBuilder.ToString());
        }

        private void GetLogonInfo(HttpContext context)
        {
            string logonUser = context.Session["logonUser"] == null ? "" : context.Session["logonUser"].ToString();
            string logonUserType = context.Session["logonUserType"] == null ? "" : context.Session["logonUser"].ToString();
            WriteResponse(context, 0, "查询成功", "\"logonUser\":\"" + logonUser + "\",\"logonUserType\":\"" + logonUserType + "\"");
        }

        private void SetLononInfo(HttpContext context)
        {
            if (context.Request.QueryString["logonUser"] != null)
                context.Session["logonUser"] = context.Request.QueryString["logonUser"].ToString();
            if (context.Request.QueryString["logonUserType"] != null)
                context.Session["logonUserType"] = context.Request.QueryString["logonUserType"].ToString();
            WriteResponse(context, 0, "操作成功", "");
        }

        private void UpdateChoiceAnswers(HttpContext context) {

            string choices = ReadFormStr(context,"answer");
            string userid = ReadFormStr(context, "userid");
            string paperid = ReadFormStr(context, "paperid");

            string sqlFormat = "update tb_check_paper set choices=\"{0}\" where user_id={1} and paper_id={2}";

            string sql = string.Format(sqlFormat,choices,userid,paperid);

            int code = ExecuteNoQuery(sql);
            if(code < 0)
                WriteResponse(context, -1, dbError);
            else
                WriteResponse(context, 0);
        }

        private void UpdateFillingAnswers(HttpContext context) {

            string answer = ReadFormStr(context, "answer");
            string userid = ReadFormStr(context, "userid");
            string paperid = ReadFormStr(context, "paperid");

            string sqlFormat = "update tb_check_paper set fillings=\"{0}\" where user_id={1} and paper_id={2}";

            string sql = string.Format(sqlFormat, answer, userid, paperid);

            int code = ExecuteNoQuery(sql);
            if (code < 0)
                WriteResponse(context, -1, dbError);
            else
                WriteResponse(context, 0);
        }

        private void UpdateJudgeAnswers(HttpContext context) {

            string answer = ReadFormStr(context, "answer");
            string userid = ReadFormStr(context, "userid");
            string paperid = ReadFormStr(context, "paperid");

            string sqlFormat = "update tb_check_paper set judges=\"{0}\" where user_id={1} and paper_id={2}";

            string sql = string.Format(sqlFormat, answer, userid, paperid);

            int code = ExecuteNoQuery(sql);
            if (code < 0)
                WriteResponse(context, -1,dbError);
            else
                WriteResponse(context, 0);
        }

        private void UpdateQaAnswers(HttpContext context) {

            string answer = ReadFormStr(context, "answer");
            string userid = ReadFormStr(context, "userid");
            string paperid = ReadFormStr(context, "paperid");

            string sqlFormat = "update tb_check_paper set qas=\"{0}\" where user_id={1} and paper_id={2}";

            string sql = string.Format(sqlFormat, answer, userid, paperid);

            int code = ExecuteNoQuery(sql);
            if (code < 0)
                WriteResponse(context, -1, dbError);
            else
                WriteResponse(context, 0);
        }

        private void test(HttpContext context)
        {
            Thread.Sleep(3000);

            int c = ExecuteQueryDataCount("select * from tb_choice");

            WriteResponse(context, 0, "操作成功", c.ToString());
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