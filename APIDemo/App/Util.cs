﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace APIDemo.App
{
    internal class Util
    {
        private static ILogger myLogger = new EventLogger();

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
                myLogger.Log(getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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
                myLogger.Log(getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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

        /// <summary>
        /// 取得MD5加密字串
        /// </summary>
        /// <param name="source">來源</param>
        /// <returns>加密字串</returns>
        public static string getMD5(string source)
        {
            byte[] original = Encoding.UTF8.GetBytes(source);  //將字串來源轉為byte[] 
            MD5 s1 = MD5.Create();
            byte[] change = s1.ComputeHash(original);
            return BitConverter.ToString(change).Replace("-", "");  //將加密後的字串從byte[]轉回string
        }

        /// <summary>
        /// 截斷字串的前N個字元
        /// </summary>
        /// <param name="str">原字串</param>
        /// <param name="maxLength">最大長度</param>
        /// <returns>截斷後的字串</returns>
        public static string truncateString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        /// <summary>
        /// 切割List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="origin">來源List</param>
        /// <param name="size">切割大小</param>
        /// <returns>切割後的List</returns>
        public static IEnumerable<List<T>> splitList<T>(List<T> origin, int size)
        {
            for (int i = 0; i < origin.Count; i += size)
            {
                yield return origin.GetRange(i, Math.Min(size, origin.Count - i));
            }
        }

        /// <summary>
        /// 檢查是否大於0
        /// </summary>
        /// <param name="d">數字</param>
        /// <returns>true: 是, false: 否</returns>
        public static bool checkMoreThanZero(decimal? d)
        {
            if (d != null && d > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否為英文字元
        /// </summary>
        /// <param name="c">字元</param>
        /// <returns>true: 是, false: 否</returns>
        public static bool isEnglishChar(char c)
        {
            int i = c;
            if ((i > 64 && i < 91) || (i > 96 && i < 123))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字串反轉(Array.Reverse)
        /// </summary>
        /// <param name="s">字串</param>
        /// <returns>反轉後的字串</returns>
        public static string ReverseByArray(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// 字串反轉(StringBuilder)
        /// </summary>
        /// <param name="s">字串</param>
        /// <returns>反轉後的字串</returns>
        public static string ReverseByStringBuilder(string s)
        {
            var sb = new StringBuilder();
            for (var i = s.Length - 1; i >= 0; i--)
            {
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字串反轉(CharBuffer)
        /// </summary>
        /// <param name="s">字串</param>
        /// <returns>反轉後的字串</returns>
        public static string ReverseByCharBuffer(string s)
        {
            char[] c = s.ToCharArray();
            for (int i = 0; i < s.Length / 2; i++)
            {
                char t = s[i];
                c[i] = s[s.Length - i - 1];
                c[s.Length - i - 1] = t;
            }
            return new string(c);
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// 安全版ToLower()
        /// </summary>
        /// <param name="s">字串</param>
        /// <returns>字串的小寫</returns>
        public static string NullSafeToLower(this string s)
        {
            if (s == null)
            {
                s = string.Empty;
            }
            return s.ToLower();
        }

        /// <summary>
        /// 取代字串null為程式中的null
        /// </summary>
        /// <param name="s">字串</param>
        /// <returns>取代後的字串</returns>
        public static string ReplaceNull(this string s)
        {
            return Const.Null.Equals(s, StringComparison.CurrentCultureIgnoreCase) ? null : s;
        }
    }
}