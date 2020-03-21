using System;


namespace Transformation
{
    class Program
    {
        static void Main(string[] args)
        {
            string str1 = "1 + 2 * 3 + 4";
            string str2 = "A or not (B and(C or D))";
            string str3 = "(not (x and y))";

            Console.WriteLine(RPNA.GetExpressionArif(str1));
            Console.WriteLine(RPNL.GetExpressionLog(str2));
            Console.WriteLine(RPNL.GetExpressionLog(str3));

            Console.ReadKey();
        }
    }
}