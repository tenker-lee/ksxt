using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ksxt
{
    public partial class changePassword : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s_o = txtOldPass.Text.Trim();
            string s_n = txtNewPass.Text.Trim();
            string s_n_c = txtNewPassConfim.Text.Trim();

            if(s_o == "")           
                return;           
            
            if(s_o=="" || s_n=="" || s_n_c == "")
            {
                Response.Write("<script>alert('输入参数有误!!!');</script>");
                return;
            }

            if(s_n != s_n_c)
            {
                Response.Write("<script>alert('新密码不一致!!!');</script>");
                return;
            }

            string sqlFormat = "select * from tb_users where name='{0}' and password='{1}'";

            string sql = string.Format(sqlFormat, logonUser, s_o);

            DataTable dt = dbBase.ExecuteQueryData(sql);

            if (dt.Rows.Count == 0)
            {
                Response.Write("<script>alert('旧密码不正确!!!')</script>");
                return;
            }
            sqlFormat = "update tb_users set  password='{0}' where name='{1}'";

            sql = string.Format(sqlFormat, s_n, logonUser);

            int code = dbBase.ExecuteNoQuery(sql);

            if(code >= 0)
            {
                Response.Write("<script>alert('修改密码成功!');window.opener=null;window.close();</script>");// 不会弹出询问
                return;
            }
            else
            {
                Response.Write("<script>alert('" + dbBase.dbError +"')</script>");
                return;
            }
        }
    }
}