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
                "b = a + b",
                "print(b)"
            };
            MyLanguage lang = new MyLanguage(prog);
            lang.Parse();
        }
    }
}
