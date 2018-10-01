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

    internal class Operator
    {
        public const string equal = "equal";
        public const string like = "like";
        public const string In = "in";
        public const string moreThan = "moreThan";
        public const string moreThanOrEqual = "moreThanOrEqual";
        public const string lessThan = "lessThan";
        public const string lessThanOrEqual = "lessThanOrEqual";
        public const string between = "between";
    }
}