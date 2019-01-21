using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;

namespace ksxt
{
    public abstract class HandleBase:dbBase
    {
        protected string logonUser { set; get; } = "";

        protected string logonUserType { set; get; } = "";

        protected void PreProcess(HttpContext context)
        {
            if (!checkPermission(context))
                WriteResponse(context ,-2, "权限验证失败", "");
            string opt = context.Request.QueryString["opt"];
            if (opt == null)
                opt = "";
            if (opt == "query")          
                Search(context);
            else if (opt == "add")
                Add(context);
            else if (opt == "edit")
                Edit(context);
            else if (opt == "del")
                Delete(context);
            else
            {
                Type thisType = this.GetType();
                MethodInfo method = thisType.GetMethod(opt,BindingFlags.IgnoreCase|BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public);
                if (method == null){
                    Default(context);
                }
                else{
                    method.Invoke(this, new object[] { context});
                }
            }
        }

        protected bool checkPermission(HttpContext context)
        {
            logonUser = context.Session["logonUser"]==null?"": context.Session["logonUser"].ToString();
            logonUserType = context.Session["logonUserType"] == null ? "" : context.Session["logonUser"].ToString();
            if (logonUserType != "2")
            {
                WriteResponse(context,-2, "权限错误,无法进行操作", "");
                //return false;
            }
            return true;
        }

        protected void WriteResponse(HttpContext context,int stateCode, string msg="操作成功", string usrStr="")
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

            if(usrStr!=null && usrStr != "")
            {
                stringBuilder.Append(",");
                stringBuilder.Append(usrStr);
            }

            stringBuilder.Append("}");

            stringBuilder.Replace("\r\n", "<br/>");
            stringBuilder.Replace("\r", "<br/>");
            stringBuilder.Replace("\n", "<br/>");

            context.Response.Write(stringBuilder.ToString());
        }

        protected string ReadFormStr(HttpContext context,string itemName)
        {
            if (context.Request.Form[itemName] == null)
                return "";
            else
                return context.Request.Form[itemName];
        }

        protected abstract void Add(HttpContext context);

        protected abstract void Edit(HttpContext context);

        protected abstract void Delete(HttpContext context);

        protected abstract void Search(HttpContext context);

        protected abstract void Default(HttpContext context);


    }
}