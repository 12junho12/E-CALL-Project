using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using log4net;

namespace AUA.AiS_FruiT
{
    public class SQLiteDataBase
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        //string ConnectionString = "Data Source= " + @"\Database\AUA_DB.db;Version=3;";
        string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;

        string ConnectionString = "Data Source= " + System.AppDomain.CurrentDomain.BaseDirectory + @"Database\AUA_DB.db;Version=3;";

        public bool ExecuteQuery(string sql)
        {
            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, c))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (System.Exception ex)
                    {
                        eLogger.Error(ex.ToString());
                    }
                }
            }
            return true;
        }

        public DataSet GetDataSet(String sql)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    try
                    {
                        adapter.SelectCommand = new SQLiteCommand(sql, c);
                        adapter.Fill(ds);
                    }
                    catch (System.Exception ex)
                    {
                        eLogger.Error(ex.ToString());
                    }
                }
            }
            return ds;
        }

        public int GetCount(String sql)
        {
            int result = 0;

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, c))
                {
                    try
                    {
                        result = (int)command.ExecuteScalar();
                    }
                    catch (System.Exception ex)
                    {
                        eLogger.Error(ex.ToString());
                    }
                }
            }
            return result;
        }

        public string GetDataReader(string sql)
        {
            String getData = "";
            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read() != false)
                            {
                                getData = reader["DATA1"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            eLogger.Error(ex.ToString());
                        }

                    }
                }
            }
            return getData;
        }

        public DataTable GetDBTable(string sql)
        {
            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, c))
                {
                    using (SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            eLogger.Error(ex.ToString());
                        }
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool SetInsertWorkList(string id, string contents, string detailCon, string model, string em_no)
        {
            bool isSuccess = false;
            string sql = "INSERT INTO [USER_WORKLIST]([ID],[UPDATE_DATETIME],[CONTENTS],[CONTENTS_DETAIL],[MODEL],[SUB_MODEL],[EMPLOYEE_NO])" +
                            "VALUES('" + id + "',datetime('now', 'localtime'),'" + contents + "','" + detailCon + "','" + model + "','','" + em_no + "')";
            isSuccess = ExecuteQuery(sql);
            return isSuccess;
        }

    }

    public class ResultSearch
    {
        public string WORK_DATE = "";
        public string MODEL = "";
        public string SCAN_BARCODE = "";
        public string OUT_BARCODE = "";
        public string RESULT = "";
        public string ID = "";
        public string NAME = "";
        public string MEAS1 = "";
        public string MEAS2 = "";
        public string MEAS3 = "";
        public string MEAS4 = "";
        public string MEAS5 = "";

    }
}
