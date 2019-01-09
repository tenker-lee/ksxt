using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using ksxt;
using System.Data;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerSingle 的摘要说明
    /// </summary>
    public class HandlerSingle :HandleBase, IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {

            ExecuteQueryData("select * from tb_choice");

            PreProcess(context);
        }
        
        public void CheckSession(HttpContext context)
        {
            string str = context.Session["logonUser"].ToString();
            return;
        }

        override protected void Add(HttpContext context) { }

        override protected void Edit(HttpContext context) { }

        override protected void Delete(HttpContext context) { }

        override protected void Search(HttpContext context) { }

        override protected void Default(HttpContext context) {
            WriteResponse(context, 0, "null","\"total\":0,\"rows\":[]");
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