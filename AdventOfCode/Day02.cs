using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;



public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{NumberOfSaveReports()}");

    public override ValueTask<string> Solve_2() => new($"{NumberOfSaveReports2()}");

    enum ListSortType {Inc, Dec, Not, Unknown};
    int NumberOfProblemsInCurrentList = 0;

    public int NumberOfSaveReports()
    {
        int runningTotal = 0;
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ListSortType listSortType = ListSortType.Unknown;
            int lastNumber = -1;
            //Check Each List
            List<int> tempList = new List<int>();

            foreach (string number in numbers)
            {
                tempList.Add(int.Parse(number));
            }
            if (CheckList(tempList))
            {
                runningTotal++;
            }
        }

        return runningTotal;
    }

    public int NumberOfSaveReports2()
    {
        int runningTotal = 0;
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            NumberOfProblemsInCurrentList = 0;
            //Check Each List
            List<int> levels = new List<int>();
            foreach (string number in numbers)
            {
                levels.Add(int.Parse(number));
            }
            if (CheckList(levels))
            {
                runningTotal++;
            }
            //Check problem damper
            else
            {
                bool worksWithProblemDamper = false;
                for (int i = 0; i < levels.Count; i++)
                {
                    List<int> tempList = new List<int>();
                    for (int j = 0; j < levels.Count; j++)
                    {
                        if (i != j){
                            tempList.Add(levels[j]);
                        }
                    }
                    if (CheckList(tempList))
                    {
                        worksWithProblemDamper = true;
                        break;
                    }
                }
                if (worksWithProblemDamper) runningTotal++;
            }

        }
        return runningTotal;
    }

    bool CheckList(List<int> levelsList)
    {
        ListSortType listSortType = ListSortType.Unknown;
        int lastNumber = -1;
        //Check Each List
        foreach (int number in levelsList)
        {
            int currentNumber = number;
            if (lastNumber == -1)
            {
                if (levelsList.Count == 1)
                {
                    Console.WriteLine("one liner");

                    listSortType = ListSortType.Dec;
                    break;
                }
                lastNumber = currentNumber;
                continue;
            }

            if (!CheckDifference(currentNumber, lastNumber) || listSortType == ListSortType.Not)
            {
                listSortType = ListSortType.Not;
                continue;
            }

            //Check if decreasing
            if (currentNumber < lastNumber)
            {
                if (listSortType == ListSortType.Dec || listSortType == ListSortType.Unknown)
                {
                    listSortType = ListSortType.Dec;
                }
                else
                {
                    listSortType = ListSortType.Not;
                }

            }
            //Check if increasing
            else if (currentNumber > lastNumber)
            {
                if (listSortType == ListSortType.Inc || listSortType == ListSortType.Unknown)
                {
                    listSortType = ListSortType.Inc;
                }
                else
                {
                    listSortType = ListSortType.Not;
                }

            }
            else
            {
                listSortType = ListSortType.Not;
            }
            lastNumber = currentNumber;
        }
        //Check if list contributes to count
        if (listSortType != ListSortType.Not)
        {
            return true;
        }
        return false;
    }

    bool CheckDifference(int a, int b)
    {
        int x = Math.Abs(b - a);
        if (x < 4 && x > 0)
        {
            return true;
        }
        return false;
    }
}
