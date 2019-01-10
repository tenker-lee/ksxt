using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ksxt
{
    public abstract class HandleBase:dbBase
    {
        protected void PreProcess(HttpContext context)
        {
            string opt = context.Request.QueryString["opt"];
            if (opt == null)
                opt = "";
            if (opt == "query")
            {
                Search(context);
            }
            else if (opt == "add")
            {
                Add(context);
            }
            else if (opt == "edit")
            {
                Edit(context);
            }
            else if (opt == "del")
            {
                Delete(context);
            }
            else
            {
                Default(context);
            }
        }

        protected void WriteResponse(HttpContext context,int stateCode, string msg, string usrStr)
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