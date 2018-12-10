namespace APIDemo.App
{
    public class Fibonacci
    {
        public int Fibonacci_For_Loop(int n)
        {
            int a = 0;
            int b = 1;

            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b += temp;
            }

            return a;
        }

        public int Fibonacci_Recursive(int a, int b, int count, int number)
        {
            int temp = a;

            if (count < number)
            {
                temp = Fibonacci_Recursive(b, a + b, count + 1, number);
            }

            return temp;
        }
    }
}