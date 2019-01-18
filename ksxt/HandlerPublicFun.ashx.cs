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

        private void WriteResponse(HttpContext context, int stateCode, string msg, string usrStr)
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