using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace APIDemo.App
{
    /// <summary>
    /// 資料庫通用函數
    /// </summary>
    internal class DbUtil
    {
        public const int maxRowSize = 4194304;  //iisexpress.exe若為32bit，DataTable記憶體使用最大不能超過2GB

        #region ADO.NET
        /// <summary>
        /// 取得參數名稱
        /// </summary>
        /// <param name="length">長度</param>
        /// <returns>參數名稱陣列 (@param0, @param1, @param2, @param3...)</returns>
        private static string[] getParamName(int length)
        {
            string[] paramName = new string[length];
            string name = "param";
            for (int i = 0; i < length; i++)
            {
                paramName[i] = "@" + name + i;
            }
            return paramName;
        }

        /// <summary>
        /// 取得參數值
        /// </summary>
        /// <param name="paramValue">參數值陣列</param>
        /// <returns>參數值陣列 (null設為空值)</returns>
        private static string[] getParamValue(string[] paramValue)
        {
            for (int i = 0; i < paramValue.Length; i++)
            {
                if (paramValue[i] == null)  //由於@param裡面不能放null，會發生例外，故改成空值
                {
                    paramValue[i] = "";
                }
            }
            return paramValue;
        }

        /// <summary>
        /// 取得完整sql
        /// </summary>
        /// <param name="sql">含@param的sql</param>
        /// <param name="paramValue">參數值陣列</param>
        /// <returns>完整sql</returns>
        private static string getFullSql(string sql, string[] paramValue)
        {
            for (int i = 0; i < paramValue.Length; i++)
            {
                var regex = new Regex(Regex.Escape("@param" + i));
                sql = regex.Replace(sql, "'" + paramValue[i] + "'", 1);  //只取代@param1，而不會取代@param11
            }
            return sql;
        }

        /// <summary>
        /// 執行sql回傳DataTable (SqlConnection每次均開啟關閉)
        /// </summary>
        /// <param name="sql">sql語法</param>
        /// <param name="paramValue">參數值陣列</param>
        /// <param name="ConnStr">連線字串</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteSql(string sql, string[] paramValue, string ConnStr)
        {
            var dt = new DataTable();
            var sc = new SqlConnection(ConnStr);

            try
            {
                dt = ExecuteSql(sql, paramValue, sc);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }
            finally
            {
                sc.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// 執行sql回傳DataTable (SqlConnection持續使用)
        /// </summary>
        /// <param name="sql">sql語法</param>
        /// <param name="paramValue">參數值陣列</param>
        /// <param name="sc">sql連線</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteSql(string sql, string[] paramValue, SqlConnection sc)
        {
            var dt = new DataTable();
            var cmd = new SqlCommand();

            try
            {
                string[] paramName = getParamName(paramValue.Length);
                paramValue = getParamValue(paramValue);
                sql = String.Format(sql, paramName);

                cmd = new SqlCommand(sql, sc);

                if (sc.State == ConnectionState.Closed)
                {
                    sc.Open();
                }
                cmd.CommandTimeout = 500;
                cmd.Connection = sc;

                if (paramName.Length != paramValue.Length)
                {
                    throw new Exception("paramName and paramValue is not mapping.");
                }

                for (int i = 0; i < paramValue.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }

                var ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
            }
            catch (SqlException e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString() + ", sql: " + getFullSql(sql, paramValue)), EventLogEntryType.Error);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }
            finally
            {
                cmd.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// 執行sql不回傳值 (僅顯示狀態碼)
        /// (SqlConnection每次均開啟關閉)
        /// </summary>
        /// <param name="sql">sql語法</param>
        /// <param name="paramValue">參數值陣列</param>
        /// <param name="ConnStr">連線字串</param>
        /// <returns>0: 影響列數等於0, 1: 影響列數大於0 (成功), -1: 發生SqlException, -2: 發生其他Exception</returns>
        public static int ExecuteSqlNoReturn(string sql, string[] paramValue, string ConnStr)
        {
            int nRet = 0;
            var sc = new SqlConnection(ConnStr);

            try
            {
                nRet = ExecuteSqlNoReturn(sql, paramValue, sc);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }
            finally
            {
                sc.Dispose();
            }

            return nRet;
        }

        /// <summary>
        /// 執行sql不回傳值 (僅顯示狀態碼)
        /// (SqlConnection持續使用)
        /// </summary>
        /// <param name="sql">sql語法</param>
        /// <param name="paramValue">參數值陣列</param>
        /// <param name="sc">sql連線</param>
        /// <returns>0: 影響列數等於0, 1: 影響列數大於0 (成功), -1: 發生SqlException, -2: 發生其他Exception</returns>
        public static int ExecuteSqlNoReturn(string sql, string[] paramValue, SqlConnection sc)
        {
            int nRet = 0;
            var cmd = new SqlCommand();

            try
            {
                string[] paramName = getParamName(paramValue.Length);
                paramValue = getParamValue(paramValue);
                sql = String.Format(sql, paramName);

                cmd = new SqlCommand(sql, sc);

                if (sc.State == ConnectionState.Closed)
                {
                    sc.Open();
                }
                cmd.CommandTimeout = 500;
                cmd.Connection = sc;

                if (paramName.Length != paramValue.Length)
                {
                    throw new Exception("paramName and paramValue is not mapping.");
                }

                for (int i = 0; i < paramValue.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }

                nRet = cmd.ExecuteNonQuery() > 0 ? 1 : 0;  //ExecuteNonQuery()只顯示update, insert, delete的影響資料列數，select會顯示-1
            }
            catch (SqlException e)
            {
                nRet = -1;
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString() + ", nRet: " +
                    nRet + ", sql: " + getFullSql(sql, paramValue)), EventLogEntryType.Error);
            }
            catch (Exception e)
            {
                nRet = -2;
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString() + ", nRet: " +
                    nRet), EventLogEntryType.Error);
            }
            finally
            {
                cmd.Dispose();
            }
            return nRet;
        }
        #endregion

        #region SqlBulkCopy
        /// <summary>
        /// SqlBulkCopy insert資料 (最快)
        /// </summary>
        /// <param name="tableName">資料表名稱</param>
        /// <param name="columnNames">欄位名稱陣列</param>
        /// <param name="dt">資料表</param>
        /// <param name="connStr">連線字串</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public static bool MySqlBulkCopy(string tableName, string[] columnNames, DataTable dt, string connStr)
        {
            bool result = false;
            var sc = new SqlConnection(connStr);

            try
            {
                if (sc.State == ConnectionState.Closed)
                {
                    sc.Open();
                }

                var sqlBC = new SqlBulkCopy(sc);
                sqlBC.BatchSize = 5000;  //魔術數字
                sqlBC.BulkCopyTimeout = 60;  //秒
                sqlBC.DestinationTableName = tableName;
                foreach (string columnName in columnNames)
                {
                    sqlBC.ColumnMappings.Add(columnName, columnName);
                }

                sqlBC.WriteToServer(dt);
                sqlBC.Close();
                result = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }
            finally
            {
                sc.Dispose();
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 設定DataTable的欄位
        /// </summary>
        /// <param name="columnNames">欄位名稱</param>
        /// <param name="columnTypes">欄位型態</param>
        /// <returns>DataTable</returns>
        public static DataTable setDataColumn(string[] columnNames, Type[] columnTypes)
        {
            if (columnNames.Length != columnTypes.Length)
            {
                throw new ArgumentException("columnNames and columnTypes are not mapping.");
            }

            var dt = new DataTable();

            //DataTable的欄位
            for (int i = 0; i < columnNames.Length; i++)
            {
                dt.Columns.Add(columnNames[i], columnTypes[i]);
            }

            return dt;
        }

        #region TVP(Table Value Parameter)
        /// <summary>
        /// TVP處理(insert or update or delete資料)
        /// </summary>
        /// <param name="sql">sql語法</param>
        /// <param name="typeName">User-Defined Table Type的名稱</param>
        /// <param name="dt">資料表</param>
        /// <param name="connStr">連線字串</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public static bool TVP_process(string sql, string typeName, DataTable dt, string connStr)
        {
            bool result = false;
            var sc = new SqlConnection(connStr);
            var cmd = new SqlCommand();

            try
            {
                if (sc.State == ConnectionState.Closed)
                {
                    sc.Open();
                }

                cmd = new SqlCommand(sql, sc);
                cmd.CommandTimeout = 500;
                SqlParameter pTVP = cmd.Parameters.Add("@" + typeName, SqlDbType.Structured);
                pTVP.Value = dt;
                pTVP.TypeName = typeName;

                result = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }
            finally
            {
                cmd.Dispose();
                sc.Dispose();
            }

            return result;
            #endregion
        }
    }
}