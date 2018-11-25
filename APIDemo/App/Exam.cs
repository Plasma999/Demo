using System;
using System.Collections.Generic;

namespace APIDemo.App_Code
{
    public class Exam
    {
        public string StringReverse()
        {
            DateTime time_start = DateTime.Now;
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int num = 10000000;
            string result = "str: " + str + ", test number: " + num + "<br/>";

            for (int i = 0; i < num; i++)
            {
                Util.ReverseByArray(str);
            }

            DateTime time_array_done = DateTime.Now;
            result += "ReverseByArray costs " + Util.getSecond(time_start, time_array_done) + " sec, ";

            for (int i = 0; i < num; i++)
            {
                Util.ReverseByStringBuilder(str);
            }

            DateTime time_stringBuilder_done = DateTime.Now;
            result += "ReverseByStringBuilder costs " + Util.getSecond(time_array_done, time_stringBuilder_done) + " sec, ";

            for (int i = 0; i < num; i++)
            {
                Util.ReverseByCharBuffer(str);
            }

            DateTime time_charBuffer_done = DateTime.Now;
            result += "ReverseByCharBuffer costs " + Util.getSecond(time_stringBuilder_done, time_charBuffer_done) + " sec, ";

            DateTime time_end = DateTime.Now;
            result += "total costs " + Util.getSecond(time_start, time_end) + " sec.";

            return result;
        }

        public string TransactionRollBack()
        {
            var transactions = new List<ITransaction>();
            transactions.Add(new StudentProfileInsert());
            transactions.Add(new StudentProfileInsert2());

            string result = "use TransactionScope: <br>";
            var t = new TransactionList(transactions);
            if (!t.ExecuteAll())
            {
                result += t.ErrMsg;
            }

            result += "<br>EF Insert: <br>";
            var s = new StudentProfileInsert3();
            if (!s.Execute())
            {
                result += s.ErrMsg;
            }

            return result;
        }
    }
}