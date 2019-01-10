using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data;

namespace ksxt
{
    public abstract class dbBase
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public static string dbError { set; get; }

        /// <summary>
        /// 数据连接,一直连接
        /// </summary>
        private static SQLiteConnection dbConnection;
        /// <summary>
        /// SQL命令定义
        /// </summary>
        private static SQLiteCommand dbCommand;

        /// <summary>
        /// 数据读取定义
        /// </summary>
        private static SQLiteDataReader dataReader;

        private static void OpenDataBase()
        {
            if (dbConnection == null)
                dbConnection = new SQLiteConnection("Data Source = " + System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Data\\ksxt.db");
            if(dbConnection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    dbConnection.Open();
                }
                catch
                {
                    dbError = "open db failed!";
                }
            }
        }

        public static int ExecuteNoQuery(string queryString)
        {
            OpenDataBase();
            int code = 0;
            try
            {
                dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = queryString;
                code = dbCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dbError = e.Message;
            }
            dbCommand.Cancel();

            return code;
        }

        public static SQLiteDataReader ExecuteQuery(string queryString)
        {
            OpenDataBase();
            try
            {
                dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = queryString;
                dataReader = dbCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                dbError = e.Message;
                return null;
            }
            return dataReader;
        }

        public static DataTable ExecuteQueryData(string queryString)
        {
            DataTable dataTable = new DataTable();

            SQLiteDataReader dataReader = ExecuteQuery(queryString);

            if (dataReader != null)
            {
                //DataTable d = dataReader.GetSchemaTable();
                int iFieldCount = dataReader.FieldCount;
                for(int i = 0; i < iFieldCount; i++)
                {
                    dataTable.Columns.Add(dataReader.GetName(i));
                }
                while (dataReader.Read())
                {
                    DataRow dr =  dataTable.NewRow();
                    for (int i = 0; i < iFieldCount; i++)
                    {
                        dr[i] = dataReader.GetValue(i).ToString();
                    }
                    dataTable.Rows.Add(dr);
                }
                dataReader.Close();
            }
            return dataTable;
        }


    }
}