using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{SolveDay1()}");

    public override ValueTask<string> Solve_2() => new($"{CalculateSimilarityScore()}");

    public long SolveDay1()
    {
        long runningTotal = 0;
        List<long> list1 = new List<long>();
        List<long> list2 = new List<long>();
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] locationID = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            list1.Add(long.Parse(locationID[0]));
            list2.Add((long.Parse(locationID[1])));
        }
        list1.Sort();
        list2.Sort();
        for (int i = 0; i < list1.Count(); i++)
        {
            long a = list1[i];
            long b = list2[i];
            if (a > b)
            {
                runningTotal += a - b;
            }
            else
            {
                runningTotal += b - a;
            }

        }
        Console.WriteLine(runningTotal);
        return runningTotal;

    }

    public long CalculateSimilarityScore()
    {
        List<long> list1 = new List<long>();
        List<long> list2 = new List<long>();
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] locationID = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine(locationID);

            list1.Add(long.Parse(locationID[0]));
            list2.Add((long.Parse(locationID[1])));
        }

        long similarityScore = 0;

        for (int i = 0; i < list1.Count(); i++)
        {
            long currentNumber = list1[i];
            long currentNumberCount = 0;
            for (int j = 0; j < list2.Count(); j++)
            {
                if(currentNumber == list2[j])
                {
                    currentNumberCount++;
                }
            }
            similarityScore += currentNumberCount * currentNumber;
        }
        Console.WriteLine(similarityScore);
        return similarityScore;

    }
}
