﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace ksxt
{

    public class publicFun
    {

        public static bool ArryCompare(string arryStr1,string arryStr2)
        {
            bool bret = false;

            string[] arry1 = arryStr1.Split(',');
            string[] arry2 = arryStr2.Split(',');

            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();

            foreach (string s1 in arry1)
            {
                if (s1.Trim() != "")
                {
                    if(!list1.Contains(s1.Trim()))
                        list1.Add(s1.Trim());
                }
            }
            foreach (string s2 in arry2)
            {
                if (s2.Trim() != "")
                {
                    if(!list2.Contains(s2.Trim()))
                        list2.Add(s2.Trim());
                }
            }

            list1.Sort();
            list2.Sort();

            if (list1.SequenceEqual(list2))
                bret = true;

            return bret;
        }

        public static int StringToInt(string value,int default_v = 0)
        {
            int iOut;

            if (int.TryParse(value, out iOut))
            {
                return iOut;
            }
            else
            {
                return default_v;
            }
        }

        public static string DataTableToJson(DataTable dt)
        {        
            if (dt.Rows.Count == 0)
                return "[{}]";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            StringBuilder stringBuilder_rows = new StringBuilder();

            foreach(DataRow dr in dt.Rows)
            {
                stringBuilder_rows.Append("{");

                foreach (DataColumn col in dt.Columns)
                {
                    stringBuilder_rows.Append("\"");
                    stringBuilder_rows.Append(col.ColumnName);
                    stringBuilder_rows.Append("\":");

                    stringBuilder_rows.Append("\"");
                    stringBuilder_rows.Append(dr[col].ToString());
                    stringBuilder_rows.Append("\",");
                }
                if (stringBuilder_rows[stringBuilder_rows.Length - 1] == ','){
                    stringBuilder_rows.Remove(stringBuilder_rows.Length - 1, 1);
                }
                stringBuilder_rows.Append("},");

                stringBuilder.Append(stringBuilder_rows.ToString());

                //重新一行
                stringBuilder_rows.Clear();
            }

            if (stringBuilder[stringBuilder.Length - 1] == ','){
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public static string GetDateString(DateTime dateTime)
        {
            if (dateTime == null)
                return "1970-01-01 0:0:0";

            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime GetDateTimeFromStr(string strDateTime)
        {
            DateTime dateTime;
                       
            if(DateTime.TryParse(strDateTime,out dateTime)) {
                return dateTime;
            }
            else {
                return DateTime.Parse("1970-01-01 0:0:0");
            }            
        }

        public static bool IsNumeric(string strNum)
        {
            int iTest;
            if (int.TryParse(strNum, out iTest))
                return true;
            else
                return false;
        }

        public static string[] StringToArry(string str)
        {
            if (str == "")
                return new string[1];
            string[] strArry = str.Split(',');
            if (strArry.Length == 0)
            {
                string[] resutlArry = new string[1];
                resutlArry[0] = str;
                return resutlArry;
            }
            else
            {                                
                return strArry;
            }
        }

        public static string[] StringToNumArry(string str)
        {
            int iCount = 0;
            string[] arry = StringToArry(str);
            if (arry == null)
                return null;
            string[] resutlArry = new string[arry.Length];
            for (int i = 0; i < arry.Length; i++)
            {
                if (IsNumeric(arry[i]))
                {
                    iCount++;
                    resutlArry[i] = arry[i];
                }
            }
            if (iCount == resutlArry.Length)
                return resutlArry;
            else
                return null;
        }

        public static string ArryToString(string[] arry)
        {
            if (arry.Length == 0)
                return "";
            string strArry = "";
            foreach (string item in arry)
            {
                strArry += item + ",";
            }
            strArry.Remove(strArry.Length - 1);
            return strArry;
        }

    }

}