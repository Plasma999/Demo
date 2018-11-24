using System.Collections.Generic;

namespace APIDemo.App_Code
{
    internal class Const
    {
        public const string AP_ID = "APIDemo";
        public const string connID = "StudentDB_ConnStr";
        public const string Null = "null";
        public const string percentage = "percentage";
        public static readonly List<string> Gender = new List<string> { "M", "F" };
        public static readonly List<string> Blood = new List<string> { "A", "B", "AB", "O" };
    }
}