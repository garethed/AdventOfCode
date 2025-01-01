using System.Drawing;

namespace AdventOfCode.AoC2023;

public class Day13
{
    [Solution(2023,13,1)]
    [Test(10, testData2)]
    [Test(405,testData)]
    public long Part1(string input)
    {
        var grids = input.Split("\n\n").Select(s => new CharGrid2(s));

        return grids.Sum(g => findReflection(g, true).Sum() + 100 * findReflection(g, false).Sum());        
    }

    [Solution(2023,13,2)]
    [Test(400,testData)]
    public long Part2(string input)
    {
        var grids = input.Split("\n\n").Select(s => new CharGrid2(s));

        return grids.Sum(g => findReflection(g, true, 1).Sum() + 100 * findReflection(g, false, 1).Sum());        
    }        

    private IEnumerable<int> findReflection(CharGrid2 grid, bool vertical, int targetDifferences = 0)
    {
        var start = vertical ? new Point2(1, 0) : new Point2(0, 1);
        var delta = new Point2(start.y, start.x);
        var current = start;

        while (grid.Contains(current))
        {
            var differenceCount = 0;
            var cross = current;

            while (grid.Contains(cross) && differenceCount <= targetDifferences)
            {
                var test1 = cross - start;
                var test2 = cross;

                while (true)
                {
                    if (!grid.Contains(test1) || !grid.Contains(test2)) break;
                    if (grid[test1] != grid[test2]) 
                    {
                        differenceCount++;
                    }
                    test1 -= start;
                    test2 += start;
                }

                cross += delta;
            }

            if (differenceCount == targetDifferences) 
            {
                //Utils.WriteLine($"found reflection at {current}", ConsoleColor.Yellow);
                yield return current.x + current.y;
            }

            current += start;
        }        

    }

    private const string testData = @"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#
";

private const string testData2 = @"#......#...
.#.#.##....
##..####.##
...##....##
..#...##...
#..#.......
##..#.#..##
###..#.####
###..#.#.##";
}
