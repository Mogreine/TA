using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_Lab
{
    public class ArithmeticEquation
    {
        public string equation;
        private StringBuilder equationPA;

        public ArithmeticEquation(string equation)
        {
            this.equation = equation.Trim();
            equationPA = new StringBuilder();
        }

        private Boolean ParseEquation()
        {
            string[] elems = equation.Split(' ');
            string tmp;
            int numbersCount = 0;
            int operationCount = 0;
            Dictionary<string, int> priority = new Dictionary<string, int>()
            {
                ["*"] = 3,
                ["/"] = 3,
                ["+"] = 2,
                ["-"] = 2,
                ["("] = 1
            };
            Stack<string> op = new Stack<string>();
            Boolean isCorrect = true;
            //while (elems.hasMoreTokens() && isCorrect)
            for (int i = 0; i < elems.Length && isCorrect; i++)
            {
                tmp = elems[i];
                if (tmp[0] >= '0' && tmp[0] <= '9')
                {
                    if (Double.Parse(tmp, CultureInfo.InvariantCulture) > 255.0)
                    {
                        isCorrect = false;
                    }
                    else
                    {
                        equationPA.Append(tmp).Append(" ");
                        numbersCount++;
                    }
                }
                else if (priority.ContainsKey(tmp))
                {
                    if (tmp == "(")
                    {
                        op.Push(tmp);
                    }
                    else if (op.Count == 0 || priority[op.Peek()] < priority[tmp])
                    {
                        op.Push(tmp);
                        operationCount++;
                    }
                    else if (priority[op.Peek()] >= priority[tmp])
                    {
                        while (op.Count == 0 && priority[op.Peek()] >= priority[tmp])
                        {
                            equationPA.Append(op.Pop()).Append(" ");
                        }
                        op.Push(tmp);
                        operationCount++;
                    }
                }
                else if (tmp == ")")
                {
                    while (op.Count != 0 && op.Peek() != "(")
                    {
                        equationPA.Append(op.Pop()).Append(" ");
                    }
                    if (op.Count == 0)
                    {
                        isCorrect = false;
                    }
                    else
                    {
                        op.Pop();
                    }
                }
                else
                {
                    isCorrect = false;
                }
            }
            if (isCorrect)
            {
                isCorrect = numbersCount == operationCount + 1;
            }
            while (op.Count != 0 && isCorrect)
            {
                if (op.Peek() == "(")
                {
                    isCorrect = false;
                }
                equationPA.Append(op.Pop()).Append(" ");
            }
            return isCorrect;
        }

        public double? GetResult()
        {
            Stack<double> nums = new Stack<double>();
            if (ParseEquation())
            {
                string[] elems = equationPA.ToString().Split(' ');
                string tmp;
                for (int i = 0; i < elems.Length; i++)
                {
                    tmp = elems[i];
                    if (tmp == "") continue;
                    if (tmp[0] >= '0' && tmp[0] <= '9')
                    {
                        nums.Push(Double.Parse(tmp, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        switch (tmp)
                        {
                            case "+":
                                nums.Push(nums.Pop() + nums.Pop());
                                break;
                            case "-":
                                nums.Push((nums.Pop() - nums.Pop()) * -1);
                                break;
                            case "*":
                                nums.Push(nums.Pop() * nums.Pop());
                                break;
                            case "/":
                                double oper1 = nums.Pop();
                                double oper2 = nums.Pop();
                                nums.Push(oper2 / oper1);
                                break;
                        }
                    }
                }
                return nums.Pop();
            }
            else
            {
                return null;
            }
        }

    }
}