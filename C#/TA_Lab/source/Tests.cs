using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TA_Lab
{
    class Tests
    {
        public static void Main()
        {
            string[] prog = {
                "double a = 2.0",
                "double b = 3.0",
                "if (3.0 == 4.0):",
                "\ta = a + a",
                "\tb = b + b",
                "else:",
                "\tb = 10",
                "\ta = 10",
                "print(a)",
                "print(b)"
            };
            MyLanguage lang = new MyLanguage(prog);
            lang.Parse();
            /*
            string s = "2.0 + 2.0 22.0 33.4";
            ArithmeticEquation eq = new ArithmeticEquation(s);
            double? res = eq.GetResult();
            Console.WriteLine(s);
            if (res == null)
                Console.Write("В выражении содержится ошибка.");
            else
                Console.Write(String.Format("{0} = {1}", s, res.Value.ToString()));
            */
        }
    }
}
