using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ksxt
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["logonUser"] = "";
            Session["logonUserType"] = "";

            Response.Redirect("~/logon.aspx");
        }
    }
}