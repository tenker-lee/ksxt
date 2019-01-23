using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ksxt
{
    public partial class changePassword : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script>window.opener=null;window.close();</script>");// 不会弹出询问
        }
    }
}