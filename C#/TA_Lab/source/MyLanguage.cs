using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TA_Lab
{
    public class MyLanguage
    {
        private List<String> Program;
        private Dictionary<String, Double> Vars;
        private Dictionary<String, String> Patterns;
        private List<String> Input;

        public MyLanguage(IEnumerable<String> program)
        {
            Vars = new Dictionary<string, double>();
            Program = new List<String>(program);
            Input = new List<string>();
            Patterns = new Dictionary<string, string>()
            {
                { "if", @"^if\s?\(([a-z]+|(\d+\.\d+))\s?(<|>|<=|>=|==)\s?([a-z]+|(\d+\.\d+))\):$" },
                { "else", @"^else:$" },
                { "init", @"^double [a-z]+ = \d+\.\d+$" },
                { "def", @"^double [a-z]+$" },
                { "print", @"^print\((.)+\)$" },
                { "scan", @"^scan\((.)+\)$" },
                { "equation", @"^[a-z]+ = .+$" }
            };
        }

        public void Parse()
        {
            int error = 0;
            bool? condition = null;
            //bool afterIf = false;
            for (int i = 0; i < Program.Count; i++)
            {
                error = 0;
                String line = Program[i];
                if (condition != null)
                {
                    IfElseSkipping(ref line, ref condition, ref i);
                }
                line = line.Trim('\t');
                if (Regex.IsMatch(line, Patterns["init"]))
                {
                    ParseInit(line);
                }
                else if (Regex.IsMatch(line, Patterns["def"]))
                {
                    ParseDef(line);
                }
                else if (error == 0 && Regex.IsMatch(line, Patterns["if"]))
                {
                    bool? res = GetLogicResult(line);
                    if (res == null)
                        error = i + 1;
                    else
                        condition = res;                    
                }
                else if (error == 0 && Regex.IsMatch(line, Patterns["print"]))
                {
                    error = ParsePrint(line, i);
                }
                else if (error == 0 && Regex.IsMatch(line, Patterns["scan"]))
                {
                    error = ParseScan(line, i);
                }
                else if (error == 0 && Regex.IsMatch(line, Patterns["equation"]))
                {
                    error = ParseEquation(line, i);
                }
                else
                {
                    error = i + 1;
                }
                if (error != 0)
                {
                    break;
                }
            }
            if (error == 0)
            {
                Console.WriteLine("\nSuccess");
            }
            else
            {
                Console.WriteLine("\nОшибка в строке №" + error);
            }
        }

        private void IfElseSkipping(ref string line, ref bool? condition, ref int i)
        {
            if (condition.Value == false)
            {
                while (!Regex.IsMatch(line, Patterns["else"]))
                {
                    line = Program[++i];
                }
                condition = null;
                line = Program[++i];
            }
            else
            {
                if (line[0] != '\t')
                {
                    if (Regex.IsMatch(line, Patterns["else"]))
                    {
                        i++;
                        while ((line = Program[i])[0] == '\t')
                            i++;
                    }
                    else
                        condition = null;
                }
            }
        }

        private int ParseInit(String line)
        {
            String[] str = line.Split(' ');
            Vars.Add(str[1], Double.Parse(str[3], CultureInfo.InvariantCulture));
            return 0;
        }

        private int ParseDef(String line)
        {
            Vars.Add(line.Trim().Split(' ')[1], 0.0);
            return 0;
        }

        private int ParseIf(String line, int i)
        {
            bool? res = GetLogicResult(line);
            

            return 0;
        }

        private bool? GetLogicResult(string line)
        {
            string inside = new Regex(@"(.+)").Match(line).Value.Trim(new char[] { '(', ')' });

            var vars = new Regex(@"[a-z]+").Matches(line);
            foreach (var match in vars)
            {
                if (match.ToString() == "if") continue;
                if (Vars.ContainsKey(match.ToString()))
                    inside.Replace(match.ToString(), Vars[match.ToString()].ToString());
                else
                    return null;
            }
            string sign = new Regex(@"((<=)|(>=)|==|>|<)").Match(inside).Value;
            var numsMatches = new Regex(@"\d+\.\d+").Matches(inside);
            List<double> nums = new List<double>(2);
            foreach (var num in numsMatches)
            {
                nums.Add(Double.Parse(num.ToString(), CultureInfo.InvariantCulture));
            }
            bool result = false;
            switch (sign)
            {
                case "<":
                    result = nums[0] < nums[1];
                    break;
                case ">":
                    result = nums[0] > nums[1];
                    break;
                case "<=":
                    result = nums[0] <= nums[1];
                    break;
                case ">=":
                    result = nums[0] >= nums[1];
                    break;
                case "==":
                    result = nums[0] == nums[1];
                    break;
            }
            return result;
        }

        private int ParsePrint(String line, int i)
        {
            int error = 0;
            Regex p = new Regex("\\(([a-z]+|\\d+\\.\\d+)\\)");
            Match match = p.Match(line);
            if (match.Success)
            {
                String smth = match.ToString().Trim(new char[] { '(', ')' });  // .substring(match.start() + 1, match.end());
                if (Vars.ContainsKey(smth))
                {
                    Console.WriteLine(Vars[smth]);
                }
                else if (Regex.IsMatch(smth, "\\d+\\.\\d+"))
                {
                    Console.WriteLine(smth);
                }
                else
                {
                    error = i + 1;
                }
            }
            else
            {
                error = i + 1;
            }
            return error;
        }

        private int ParseScan(String line, int i)
        {
            int error = 0;
            Regex p = new Regex("\\([a-z]+\\)");
            Match match = p.Match(line);
            if (match.Success)
            {
                String smth = match.ToString().Trim(new char[] { '(', ')' });
                if (Vars.ContainsKey(smth))
                {
                    if (Input.Count == 0)
                    {
                        Input.AddRange(Console.ReadLine().Trim().Split(' '));
                    }
                    Vars[smth] = Double.Parse(Input.First(), CultureInfo.InvariantCulture);
                    Input.RemoveAt(0);
                }
                else
                {
                    error = i + 1;
                }
            }
            else
            {
                error = i + 1;
            }
            return error;
        }

        private int ParseEquation(String line, int i)
        {

            int error = 0;
            String var = line.Split(' ').First();
            if (Vars.ContainsKey(var))
            {
                String equation = new Regex("= .+$").Match(line).ToString().Trim().Substring(2);
                MatchCollection matches = new Regex(@"[a-z]+").Matches(equation);
                foreach (var match in matches)
                {
                    if (!Vars.ContainsKey(match.ToString()))
                    {
                        return i + 1;
                    }
                    else
                    {
                        equation = equation.Replace(match.ToString(), Vars[match.ToString()].ToString()).Replace(',', '.');
                    }                   
                }
                double? ans = new ArithmeticEquation(equation).GetResult();
                if (ans != null)
                {
                    Vars[var] = ans.Value;
                }
                else
                {
                    error = i + 1;
                }
            }
            else
            {
                error = i + 1;
            }
            return error;
        }

    }
}
