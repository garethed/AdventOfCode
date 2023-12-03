using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace AdventOfCode.AoC2023;

class Day3
{
    [Solution(2023,3,1)]
    [Test(4361, testData)]
    public int Par1(CharGrid2 grid)
    {
        var parts = new List<int>();
        var state = State.Outside;
        var currentNumber = 0;

        void endNumber()
        {
            if (state == State.InPart)
            {
                parts.Add(currentNumber);
            }
            state = State.Outside;
            currentNumber = 0;
        }

        bool hasAdjacentSymbol(Point2 p)
        {
            return grid.Neighbours8(p).Any(p2 => grid[p2] != '.' && !char.IsDigit(grid[p2]));
        }

        foreach (var p in grid.Points)
        {
            if (p.x == 0) { endNumber(); }
            var c = grid[p];

            if (char.IsDigit(c))
            {
                currentNumber = currentNumber * 10 + int.Parse(c.ToString());
                if (state == State.Outside) { state = State.InNumber; }
                if (state == State.InNumber && hasAdjacentSymbol(p)) { state = State.InPart; }
            }
            else
            {
                endNumber();
            }
        }

        endNumber();

        return parts.Sum();

    }

    [Solution(2023,3,2)]
    [Test(467835, testData)]
    public int Par2(CharGrid2 grid)
    {
        var gearAdjacentNumbers = new Dictionary<Point2, List<int>>();

        var parts = new List<int>();
        var state = State.Outside;
        var currentNumber = 0;
        var foundGears = new HashSet<Point2>();

        void endNumber()
        {
            if (currentNumber > 0 && foundGears.Count > 0)
            {
                foreach (var g in foundGears)
                {
                    if (!gearAdjacentNumbers.ContainsKey(g)) { gearAdjacentNumbers[g] = new List<int>(); }

                    gearAdjacentNumbers[g].Add(currentNumber);

                }
            }
            state = State.Outside;
            currentNumber = 0;
            foundGears.Clear();
        }

        IEnumerable<Point2> AdjacentGears(Point2 p)
        {
            return grid.Neighbours8(p).Where(p2 => grid[p2] == '*');
        }

        foreach (var p in grid.Points)
        {
            if (p.x == 0) { endNumber(); }
            var c = grid[p];

            if (char.IsDigit(c))
            {
                currentNumber = currentNumber * 10 + int.Parse(c.ToString());
                if (state == State.Outside) { state = State.InNumber; }
                foundGears.UnionWith(AdjacentGears(p));
            }
            else
            {
                endNumber();
            }
        }

        endNumber();

        return gearAdjacentNumbers.Where(kv => kv.Value.Count == 2).Select(kv => kv.Value[0] * kv.Value[1]).Sum();
    }    

    private enum State 
    {
        InNumber,
        InPart,
        Outside
    }

    private const string testData = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

}