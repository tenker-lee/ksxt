using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using ksxt;
using System.Data;

namespace ksxt.Admin
{

    /// <summary>
    /// HandlerSingle 的摘要说明
    /// </summary>
    public class HandlerSingle :HandleBase, IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            PreProcess(context);
        }
        
        public void CheckSession(HttpContext context)
        {
            
        }

        override protected void Add(HttpContext context)
        {
            string hid = ReadFormStr(context, "hid");
            string level = ReadFormStr(context, "level"); 
            string title = ReadFormStr(context, "singleTitle"); 
            string selectA = ReadFormStr(context, "selectA");
            string selectB = ReadFormStr(context, "selectB"); 
            string selectC = ReadFormStr(context, "selectC"); 
            string selectD = ReadFormStr(context, "selectD"); 
            string answer = ReadFormStr(context, "singleAnswer");            

            WriteResponse(context, 0, "操作成功","");
        }

        override protected void Edit(HttpContext context)
        {

            string id = ReadFormStr(context, "singleId");
            string level = ReadFormStr(context, "level");
            string title = ReadFormStr(context, "singleTitle");
            string selectA = ReadFormStr(context, "selectA");
            string selectB = ReadFormStr(context, "selectB");
            string selectC = ReadFormStr(context, "selectC");
            string selectD = ReadFormStr(context, "selectD");
            string answer = ReadFormStr(context, "singleAnswer");

            WriteResponse(context, 0, "操作成功", "");
        }

        override protected void Delete(HttpContext context)
        {
            string id = ReadFormStr(context, "singleId");


            WriteResponse(context, 0, "操作成功", "");
        }

        override protected void Search(HttpContext context)
        {

            DataTable dt = ExecuteQueryData("select * from tb_choice");            

            string dtJson = publicFun.DataTableToJson(dt);

            string listJson = "\"total\":2,\"rows\":";

            listJson += dtJson;

            WriteResponse(context, 0, "ok", listJson);
        }

        override protected void Default(HttpContext context) {
            WriteResponse(context, 0, "null","\"total\":0,\"rows\":[]");
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