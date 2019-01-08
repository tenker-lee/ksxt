using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ksxt
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public override void Init()
        {
            base.Init();

            BeginRequest += WebApiApplication_BeginRequest;
        }

        private void WebApiApplication_BeginRequest(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
