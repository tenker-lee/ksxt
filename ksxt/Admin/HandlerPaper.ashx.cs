using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ksxt.Admin
{
    /// <summary>
    /// HandlerPaper 的摘要说明
    /// </summary>
    public class HandlerPaper : HandleBase, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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
            throw new NotImplementedException();
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