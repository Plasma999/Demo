using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Configuration;

namespace APIDemo.App_Code
{
    internal class Util
    {
        ///<summary>
        ///取得除錯訊息
        ///</summary>
        ///<param name="currentMethod">目前執行的方法</param>
        ///<param name="msg">訊息</param>
        ///<returns>[類別名稱].[方法名稱]: [訊息]</returns>
        public static string getDebugMsg(MethodBase currentMethod, string msg)
        {
            return currentMethod.DeclaringType.Name + "." + currentMethod.Name + ": " + msg;
        }

        public static string getconnectionString(string connID)
        {
            string connStr = "";

            try
            {
                connStr = WebConfigurationManager.ConnectionStrings[connID].ConnectionString;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }

            return connStr;
        }

        /// <summary>
        /// 取得秒數間隔
        /// </summary>
        /// <param name="startTime">起始時間</param>
        /// <param name="endTime">結束時間</param>
        /// <returns>秒數間隔</returns>
        public static string getSecond(DateTime startTime, DateTime endTime)
        {
            double d = Math.Round((((TimeSpan)(endTime - startTime)).TotalMilliseconds / 1000), 3);  //x.xxx秒
            return d.ToString();
        }
    }
}