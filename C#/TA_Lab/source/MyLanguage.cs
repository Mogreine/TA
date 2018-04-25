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
                {"init", @"^double [a-z]+ = \d+\.\d+$" },
                {"def", @"^double [a-z]+$" },
                {"print", @"^print\((.)+\)$" },
                {"scan", @"^scan\((.)+\)$" },
                {"equation", @"^[a-z]+ = .+$" }
            };
        }

        public void Parse()
        {
            int error = 0;
            for (int i = 0; i < Program.Count; i++)
            {
                error = 0;
                String line = Program[i];
                if (Regex.IsMatch(line, Patterns["init"]))
                {
                    ParseInit(line);
                }
                else if (Regex.IsMatch(line, Patterns["def"]))
                {
                    ParseDef(line);
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
                    Console.Write(Vars[smth]);
                }
                else if (Regex.IsMatch(smth, "\\d+\\.\\d+"))
                {
                    Console.Write(smth);
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
