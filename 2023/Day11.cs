namespace AdventOfCode.AoC2023;

public class Day11
{

    [Solution(2023,11,1, 2)]
    [Test(374, testData, 2)]
    [Solution(2023,11,2, 1000000)]
    [Test(1030, testData, 10)]
    public long Part1(CharGrid2 grid, long factor)
    {
        var emptyRows = Enumerable.Range(0, grid.Height).Where(y => Enumerable.Range(0, grid.Width).All(x => grid[x,y] == '.')).ToArray();
        var emptyCols = Enumerable.Range(0, grid.Width).Where(x => Enumerable.Range(0, grid.Height).All(y => grid[x,y] == '.')).ToArray();

        var galaxies = grid.Points.Where(p => grid[p] == '#').ToArray();

        var distance = 0L;

        for (var i = 0; i < galaxies.Length - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Length; j++)
            {
                var first = galaxies[i];
                var second = galaxies[j];

                distance += Math.Abs(first.x - second.x) + Math.Abs(first.y - second.y) 
                    + (emptyRows.Count(r => first.y < r && second.y > r)
                    + emptyRows.Count(r => first.y > r && second.y < r)
                    + emptyCols.Count(r => first.x < r && second.x > r)
                    + emptyCols.Count(r => first.x > r && second.x < r)) * (factor - 1L);

            }

        }

        return distance;

    }


    private const string testData = @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
";
}