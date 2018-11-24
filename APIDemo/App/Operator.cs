namespace APIDemo.App_Code
{
    internal abstract class Operator
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    internal class Operator_equal : Operator
    {
        public Operator_equal()
        {
            Name = "equal";
            Value = "=";
        }
    }

    internal class Operator_like : Operator
    {
        public Operator_like()
        {
            Name = "like";
            Value = Name;
        }
    }

    internal class Operator_in : Operator
    {
        public Operator_in()
        {
            Name = "in";
            Value = Name;
        }
    }

    internal class Operator_moreThan : Operator
    {
        public Operator_moreThan()
        {
            Name = "moreThan";
            Value = ">";
        }
    }

    internal class Operator_moreThanOrEqual : Operator
    {
        public Operator_moreThanOrEqual()
        {
            Name = "moreThanOrEqual";
            Value = ">=";
        }
    }

    internal class Operator_lessThan : Operator
    {
        public Operator_lessThan()
        {
            Name = "lessThan";
            Value = "<";
        }
    }

    internal class Operator_lessThanOrEqual : Operator
    {
        public Operator_lessThanOrEqual()
        {
            Name = "lessThanOrEqual";
            Value = "<=";
        }
    }

    internal class Operator_between : Operator
    {
        public Operator_between()
        {
            Name = "between";
            Value = Name;
        }
    }
}