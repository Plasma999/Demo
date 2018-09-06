using System.Collections.Generic;

namespace APIDemo.App_Code
{
    internal class Const
    {
        public const string AP_ID = "APIDemo";
        public const string connID = "StudentDB_ConnStr";
        public static readonly List<string> Gender = new List<string> { "M", "F" };
        public static readonly List<string> Blood = new List<string> { "A", "B", "AB", "O" };
    }
}