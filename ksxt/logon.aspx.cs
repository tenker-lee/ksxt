using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ksxt
{
    public partial class logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(txtName.Text.Trim()=="" || txtPassword.Text.Trim() == "")
            {
               // Response.Write("<script>alert('请先登录系统!!!')</script>");

                return;
            }

            string sqlFormat = "select * from tb_users where name='{0}' and password='{1}'";

            string sql = string.Format(sqlFormat, txtName.Text.Trim(), txtPassword.Text.Trim());

            DataTable dt = dbBase.ExecuteQueryData(sql);

            if (dt.Rows.Count == 0)
            {
                Response.Write("<script>alert('帐号或密码不正确!!!')</script>");
                return;
            }

            string user = dt.Rows[0]["name"].ToString();
            string type = dt.Rows[0]["type"].ToString();

            Session["logonUser"] = user;
            Session["logonUserType"] = type;

            if (type == "1")
                Response.Redirect("~/Admin/index.aspx");
            else
                Response.Redirect("~/index.aspx");

        }
    }
}