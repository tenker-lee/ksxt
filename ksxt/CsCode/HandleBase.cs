using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ksxt
{
    public abstract class HandleBase:dbBase
    {
        protected string logonUser { set; get; } = "";

        protected string logonUserType { set; get; } = "";

        protected string logonId { set; get; } = "";


        protected void PreProcess(HttpContext context)
        {
            if (!checkPermission(context)) {
                WriteResponse(context, -2, "登录超时,请重新登录!", "");
                return;
            }
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

            logonUserType = context.Session["logonUserType"] == null ? "" : context.Session["logonUserType"].ToString();

            logonId = context.Session["logonId"] == null ? "" : context.Session["logonId"].ToString();
            
            //if (logonUserType != "1")
            //{
            //    WriteResponse(context,-2, "权限错误,无法进行操作", "");
            //    //return false;
            //}
            if(logonId=="" || logonUser == "" || logonUserType == "") {
                return false;
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
            else {
                string Htmlstring = context.Request.Form[itemName];
                if (string.IsNullOrEmpty(Htmlstring))
                    return "";
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "or", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "script", "", RegexOptions.IgnoreCase);

                //特殊的字符
                Htmlstring = Htmlstring.Replace("<", "");
                Htmlstring = Htmlstring.Replace(">", "");
                Htmlstring = Htmlstring.Replace("*", "");
                Htmlstring = Htmlstring.Replace("-", "");
                Htmlstring = Htmlstring.Replace("?", "");
                Htmlstring = Htmlstring.Replace(",", "，");
                Htmlstring = Htmlstring.Replace("/", "");
                Htmlstring = Htmlstring.Replace(";", "");
                Htmlstring = Htmlstring.Replace("*/", "");
                Htmlstring = Htmlstring.Replace("\r\n", "");
                return Htmlstring;
            }
        }

        protected abstract void Add(HttpContext context);

        protected abstract void Edit(HttpContext context);

        protected abstract void Delete(HttpContext context);

        protected abstract void Search(HttpContext context);

        protected abstract void Default(HttpContext context);


    }
}