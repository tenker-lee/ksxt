﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using ksxt;
using System.Data;
using System.Reflection;

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

        override protected void Add(HttpContext context)
        {
            string hid = ReadFormStr(context, "hid");

            string level = ReadFormStr(context, "f_level"); 
            string title = ReadFormStr(context, "f_title"); 
            string selectA = ReadFormStr(context, "f_selectA");
            string selectB = ReadFormStr(context, "f_selectB"); 
            string selectC = ReadFormStr(context, "f_selectC"); 
            string selectD = ReadFormStr(context, "f_selectD"); 
            string answer = ReadFormStr(context, "f_singleAnswer");
            if(level=="" || title=="" || selectA=="" 
                || selectB=="" || selectC==""||selectD==""||answer=="")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }

            string sqlFormat = @"insert into tb_choice(level,title,select_arry,answer_arry,create_name,create_time)values(
                                                        '{0}','{1}','{2}','{3}','{4}','{5}')";
            string selectStrs = selectA +","+ selectB + "," + selectC + "," + selectD;
            string sql = string.Format(sqlFormat, level,title, selectStrs, answer,logonUser,publicFun.GetDateString(DateTime.Now));

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        override protected void Edit(HttpContext context)
        {
            string edit_id = ReadFormStr(context, "edit_id");
            string level = ReadFormStr(context, "f_level");
            string title = ReadFormStr(context, "f_title");
            string selectA = ReadFormStr(context, "f_selectA");
            string selectB = ReadFormStr(context, "f_selectB");
            string selectC = ReadFormStr(context, "f_selectC");
            string selectD = ReadFormStr(context, "f_selectD");
            string answer = ReadFormStr(context, "f_singleAnswer");
            if (edit_id == "" || level == "" || title == "" 
                || selectA == "" || selectB == "" 
                || selectC == "" || selectD == "" || answer == "")
            {
                WriteResponse(context, -1, "输出参数有误", "");
                return;
            }
            string sqlFormat = @"update tb_choice set level='{0}',title='{1}',select_arry='{2}',answer_arry='{3}',create_name='{4}',create_time='{5}' where id={6}";
            string selectStrs = selectA + "," + selectB + "," + selectC + "," + selectD;
            string sql = string.Format(sqlFormat, level, title, selectStrs, answer, logonUser, publicFun.GetDateString(DateTime.Now),edit_id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        override protected void Delete(HttpContext context)
        {
            string id = ReadFormStr(context, "delid");

            string sqlFormat = "delete from tb_choice where id={0}";
            string sql = string.Format(sqlFormat, id);

            int code = ExecuteNoQuery(sql);

            if (code >= 0)
                WriteResponse(context, 0, "操作成功", "");
            else
                WriteResponse(context, code, dbError, "");
        }

        override protected void Search(HttpContext context)
        {
            DataTable dt = ExecuteQueryData("select * from tb_choice");
            //视图
            DataTable dtView = new DataTable();
            dtView.Columns.Add("v_id");
            dtView.Columns.Add("v_level");
            dtView.Columns.Add("v_title");
            dtView.Columns.Add("v_select_arry");
            dtView.Columns.Add("v_answer_arry");
            dtView.Columns.Add("v_create_name");
            dtView.Columns.Add("v_create_time");

            foreach(DataRow dr in dt.Rows)
            {
                DataRow newDr =  dtView.NewRow();
                newDr["v_id"] = dr["id"];
                string lev = dr["level"].ToString();
                if(lev=="1")
                    newDr["v_level"] ="初级";
                else if(lev=="2")
                    newDr["v_level"] = "中级";
                else if(lev=="3")
                    newDr["v_level"] = "高级";
                else
                    newDr["v_level"] = "无";

                newDr["v_title"] = dr["title"];

                string selStr = dr["select_arry"].ToString();
                string[] arry = publicFun.StringToArry(selStr);
                selStr = "";
                int i = 0;
                char a = 'A';
                foreach (string s in arry)
                {
                    selStr += (char)(a + i);
                    selStr += ":";
                    selStr += s;
                    selStr += "<br>";
                    i++;
                }
                newDr["v_select_arry"] = selStr;

                string anserSel =  dr["answer_arry"].ToString();
                string[] arryAswer = publicFun.StringToArry(anserSel);
                anserSel = "";
                i = 0;
                a = 'A';
                foreach (string s in arryAswer)
                {
                    int iAnser = 0;
                    if (int.TryParse(s, out iAnser))
                    {
                        if (iAnser < 5)
                        {
                            anserSel += (char)(a + iAnser);
                            anserSel += ",";
                        }
                    }
                }
                if (anserSel.Length != 0)
                    anserSel = anserSel.Remove(anserSel.Length - 1);
                newDr["v_answer_arry"] = anserSel;

                newDr["v_create_name"] = dr["create_name"];
                newDr["v_create_time"] = dr["create_time"];

                dtView.Rows.Add(newDr);
            }          
            //转JSON
            string dtJson = publicFun.DataTableToJson(dtView);

            string listJson = "\"total\":"+dt.Rows.Count+",\"rows\":";

            listJson += dtJson;

            WriteResponse(context, 0, "查询成功", listJson);
        }              

        override protected void Default(HttpContext context) {            
                WriteResponse(context, 0, "hello", "\"total\":0,\"rows\":[]");           
        }

        void SearchById(HttpContext context)
        {
            WriteResponse(context, 0, "ok", "");
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