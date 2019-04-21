using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ksxt
{
    public class CustomPage:System.Web.UI.Page
    {
        public string logonUser { set; get; }
        public string logonUserType { set; get; }
        public string logonId { set; get; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            logonUser = Session["logonUser"] == null ? "" : Session["logonUser"].ToString();
            logonUserType = Session["logonUserType"] == null ? "" : Session["logonUserType"].ToString();
            logonId = Session["logonId"] == null ? "" : Session["logonId"].ToString();

            if (logonUser == "")
                Response.Redirect("~/logon.aspx");
        }

    }
}