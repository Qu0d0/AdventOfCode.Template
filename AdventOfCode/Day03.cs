using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.IO;

namespace AdventOfCode;
public class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{MultiplySomeNumbers()}");

    public override ValueTask<string> Solve_2() => new($"{MultiplySomeNumbers2()}");

    List<string> correctStatements = new List<string>();
    long MultiplySomeNumbers()
    {
        long runningTotal = 0;
        correctStatements = new List<string>();

        string pattern = @"mul\(\d{1,3},\d{1,3}\)";
        foreach (Match match in Regex.Matches(_input, pattern, RegexOptions.IgnoreCase))
        {
            correctStatements.Add(match.Value);
        }

        foreach (string statement in correctStatements)
        {
            pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

            Match match = Regex.Match(statement, pattern);

            string a = match.Groups[1].Value;
            string b = match.Groups[2].Value;

            long num1 = long.Parse(a);
            long num2 = long.Parse(b);
            runningTotal += num1 * num2;
        }

        return runningTotal;
    }

    long MultiplySomeNumbers2()
    {
        long runningTotal = 0;
        correctStatements = new List<string>();

        string pattern = @"(mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\))";
        foreach (Match match in Regex.Matches(_input, pattern, RegexOptions.IgnoreCase))
        {
            correctStatements.Add(match.Value);
        }

        bool isEnabled = true;
        foreach (string statement in correctStatements)
        {

            if (statement == "do()")
            {
                isEnabled = true;
            }
            else if (statement == "don't()")
            {
                isEnabled = false;
            }
            else
            {
                pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
                Match match = Regex.Match(statement, pattern);
                if (isEnabled)
                {
                    string a = match.Groups[1].Value;
                    string b = match.Groups[2].Value;

                    long num1 = long.Parse(a);
                    long num2 = long.Parse(b);
                    runningTotal += num1 * num2;
                }
            }
        }
        return runningTotal;
    }
}
