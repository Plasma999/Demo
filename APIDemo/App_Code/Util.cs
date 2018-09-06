using System;
using System.Collections.Generic;
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

        public static string getAppString(string appName)
        {
            string appString = "";

            try
            {
                appString = WebConfigurationManager.AppSettings[appName];
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }

            return appString;
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

        /// <summary>
        /// 取隨機值
        /// </summary>
        /// <param name="source">來源List</param>
        /// <returns>字串</returns>
        public static string getRandom(List<string> source)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            int index = random.Next(source.Count);
            return source[index];
        }

        /// <summary>
        /// 取隨機值的List
        /// </summary>
        /// <param name="source">來源List</param>
        /// <param name="size">回傳List大小</param>
        /// <returns>回傳List</returns>
        public static List<string> getRandomList(List<string> source, int size)
        {
            var list = new List<string>();

            for (int i = 0; i < size; i++)
            {
                list.Add(getRandom(source));
            }

            return list;
        }

        /// <summary>
        /// 取隨機Decimal
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="digit">小數位數</param>
        /// <returns>Decimal</returns>
        public static decimal getRandomDecimal(double min, double max, int digit)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            double value = random.NextDouble() * (max - min) + min;
            return Convert.ToDecimal(Math.Round(value, digit));
        }

        /// <summary>
        /// 取隨機Decimal的List
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="digit">小數位數</param>
        /// <param name="size">List大小</param>
        /// <returns>回傳List</returns>
        public static List<decimal> getRandomDecimalList(double min, double max, int digit, int size)
        {
            var list = new List<decimal>();

            for (int i = 0; i < size; i++)
            {
                list.Add(getRandomDecimal(min, max, digit));
            }

            return list;
        }
    }
}