using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode;
public class Day04 : BaseDay
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{FindNumberOfXMas()}");

    public override ValueTask<string> Solve_2() => new($"{FindNumberOfXMas2()}");

    List<string> rows = new List<string>();
    int NumberOfRows = 0;
    int NumberOfCollums = 0;
    char[,] puzzle;
    struct Coordinates()
    {
        public int x = -1;
        public int y = -1;
        public int lastDirection = -1;
    }

    long FindNumberOfXMas()
    {
        InitPuzzleData();


        List<Coordinates> x_coordinates = new List<Coordinates>();
        FindAllLettersOfChar('X', ref x_coordinates);

        List<Coordinates> m_coordinates = new List<Coordinates>();
        foreach (Coordinates x_coordinate in x_coordinates)
        {
            FindAllNeighborWithLetter('M', x_coordinate, ref m_coordinates);
        }

        List<Coordinates> a_coordinates = new List<Coordinates>();
        foreach (Coordinates m_coordinate in m_coordinates)
        {
            Coordinates a_coordinate = new Coordinates();
            if (CheckNeighborLetterInDirection('A', m_coordinate, m_coordinate.lastDirection, ref a_coordinate))
            {
                a_coordinates.Add(a_coordinate);
            }
        }

        List<Coordinates> s_coordinates = new List<Coordinates>();
        foreach (Coordinates a_coordinate in a_coordinates)
        {
            Coordinates s_coordinate = new Coordinates();
            if (CheckNeighborLetterInDirection('S', a_coordinate, a_coordinate.lastDirection, ref s_coordinate))
            {
                s_coordinates.Add(a_coordinate);
            }
        }
        /*
        foreach (Coordinates m_coordinate in m_coordinates)
        {
            FindAllNeighborWithLetter('A', m_coordinate, ref a_coordinates);
        }
        List<Coordinates> s_coordinates = new List<Coordinates>();
        foreach (Coordinates a_coordinate in a_coordinates)
        {
            FindAllNeighborWithLetter('S', a_coordinate, ref s_coordinates);
        }
        */


        return s_coordinates.Count;
    }



    void InitPuzzleData()
    {
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            NumberOfRows++;
            NumberOfCollums = line.Length;
        }

        puzzle = new char[NumberOfCollums, NumberOfRows];
        int lineIndex = 0;
        foreach (string line in _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            int charIndex = 0;
            foreach (char myChar in line)
            {
                puzzle[charIndex, lineIndex] = myChar;
                charIndex++;
            }

            lineIndex++;
        }
    }

    void FindAllLettersOfChar(char myLetter, ref List<Coordinates> letterCoordinates)
    {
        for (int x = 0; x < puzzle.GetLength(0); x++)
        {
            for (int y = 0; y < puzzle.GetLength(1); y++)
            {
                if (myLetter == puzzle[x, y])
                {
                    Coordinates coordinate = new Coordinates();
                    coordinate.x = x;
                    coordinate.y = y;
                    letterCoordinates.Add(coordinate);
                }
            }
        }
    }

    bool CheckNeighborLetterInDirection(char letter, Coordinates currentCoordinate, int direction, ref Coordinates nextCoordinate)
    {
        Coordinates newCoordinate = new Coordinates();
        switch (direction)
        {
            case 0:
                newCoordinate.x = currentCoordinate.x; newCoordinate.y = currentCoordinate.y + 1;
                break;
            case 1:
                newCoordinate.x = currentCoordinate.x + 1; newCoordinate.y = currentCoordinate.y + 1;
                break;
            case 2:
                newCoordinate.x = currentCoordinate.x + 1; newCoordinate.y = currentCoordinate.y;
                break;
            case 3:
                newCoordinate.x = currentCoordinate.x + 1; newCoordinate.y = currentCoordinate.y - 1;
                break;
            case 4:
                newCoordinate.x = currentCoordinate.x; newCoordinate.y = currentCoordinate.y - 1;
                break;
            case 5:
                newCoordinate.x = currentCoordinate.x - 1; newCoordinate.y = currentCoordinate.y - 1;
                break;
            case 6:
                newCoordinate.x = currentCoordinate.x - 1; newCoordinate.y = currentCoordinate.y;
                break;
            case 7:
                newCoordinate.x = currentCoordinate.x - 1; newCoordinate.y = currentCoordinate.y + 1;
                break;
            default:
                Console.WriteLine("Number is out of range. Please enter a number between 1 and 8.");
                break;
        }
        nextCoordinate = newCoordinate;
        nextCoordinate.lastDirection = direction;
        if (!CheckBounds(newCoordinate.x, newCoordinate.y)) return false;
        if (puzzle[newCoordinate.x, newCoordinate.y] == letter) return true;
        return false;
    }

    void FindAllNeighborWithLetter(char letter, Coordinates coordinate, ref List<Coordinates> list)
    {
        List<Coordinates> neighborCoordinates = new List<Coordinates>();
        FillListWithPossibleNeighborCoordinates(coordinate, ref neighborCoordinates);
        foreach (Coordinates c in neighborCoordinates)
        {
            if (puzzle[c.x, c.y] == letter)
            {
                list.Add(c);
                //Console.WriteLine("Found " + letter + " adjesent");
            }
        }
    }

    bool CheckXAtCoordinate(Coordinates coordinate)
    {
        Coordinates newCoordinate1 = new Coordinates();
        Coordinates newCoordinate2 = new Coordinates();
        Coordinates newCoordinate3 = new Coordinates();
        Coordinates newCoordinate4 = new Coordinates();

        newCoordinate1.x = coordinate.x + 1; newCoordinate1.y = coordinate.y + 1;
        newCoordinate2.x = coordinate.x + 1; newCoordinate2.y = coordinate.y - 1;
        newCoordinate3.x = coordinate.x - 1; newCoordinate3.y = coordinate.y + 1;
        newCoordinate4.x = coordinate.x - 1; newCoordinate4.y = coordinate.y - 1;

        if (!CheckBounds(newCoordinate1.x, newCoordinate1.y)) return false;
        if (!CheckBounds(newCoordinate2.x, newCoordinate2.y)) return false;
        if (!CheckBounds(newCoordinate3.x, newCoordinate3.y)) return false;
        if (!CheckBounds(newCoordinate4.x, newCoordinate4.y)) return false;

        int count = 0;

        if (puzzle[newCoordinate1.x, newCoordinate1.y] == 'S')
        {
            if(puzzle[newCoordinate4.x, newCoordinate4.y] == 'M')
            {
                count++;
            }
        }
        if (puzzle[newCoordinate1.x, newCoordinate1.y] == 'M')
        {
            if (puzzle[newCoordinate4.x, newCoordinate4.y] == 'S')
            {
                count++;
            }
        }

        if (puzzle[newCoordinate2.x, newCoordinate2.y] == 'S')
        {
            if (puzzle[newCoordinate3.x, newCoordinate3.y] == 'M')
            {
                count++;
            }
        }
        if (puzzle[newCoordinate2.x, newCoordinate2.y] == 'M')
        {
            if (puzzle[newCoordinate3.x, newCoordinate3.y] == 'S')
            {
                count++;
            }
        }
        if (count == 2) return true;
        return false;
    }

    void FillListWithPossibleNeighborCoordinates(Coordinates coordinate, ref List<Coordinates> myList)
    {
        Coordinates newCoordinate = new Coordinates();
        //UP
        newCoordinate.x = coordinate.x; newCoordinate.y = coordinate.y + 1;
        newCoordinate.lastDirection = 0;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //UP RIGHT
        newCoordinate.x = coordinate.x + 1; newCoordinate.y = coordinate.y + 1;
        newCoordinate.lastDirection = 1;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //RIGHT
        newCoordinate.x = coordinate.x + 1; newCoordinate.y = coordinate.y;
        newCoordinate.lastDirection = 2;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //RIGHT DOWN
        newCoordinate.x = coordinate.x + 1; newCoordinate.y = coordinate.y - 1;
        newCoordinate.lastDirection = 3;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //DOWN
        newCoordinate.x = coordinate.x; newCoordinate.y = coordinate.y - 1;
        newCoordinate.lastDirection = 4;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //DOWN LEFT
        newCoordinate.x = coordinate.x - 1; newCoordinate.y = coordinate.y - 1;
        newCoordinate.lastDirection = 5;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //LEFT
        newCoordinate.x = coordinate.x - 1; newCoordinate.y = coordinate.y;
        newCoordinate.lastDirection = 6;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
        //LEFT UP
        newCoordinate.x = coordinate.x - 1; newCoordinate.y = coordinate.y + 1;
        newCoordinate.lastDirection = 7;
        if (CheckBounds(newCoordinate.x, newCoordinate.y)) myList.Add(newCoordinate);
    }

    bool CheckBounds(int x, int y)
    {
        if (x < 0 || y < 0) { return false; }
        if (puzzle.GetLength(0) <= x || puzzle.GetLength(1) <= y) { return false; }
        return true;
    }

    long FindNumberOfXMas2()
    {
        InitPuzzleData();


        List<Coordinates> a_coordinates = new List<Coordinates>();
        FindAllLettersOfChar('A', ref a_coordinates);

        List<Coordinates> m_coordinates = new List<Coordinates>();
        List<Coordinates> s_coordinates = new List<Coordinates>();
        int x_count = 0;
        foreach (Coordinates a_coordinate in a_coordinates)
        {
            if(CheckXAtCoordinate(a_coordinate)) { x_count++; }
        }

        return x_count;
    }

}


