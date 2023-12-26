using System.ComponentModel;

namespace AdventOfCode.AoC2023;

class Day14
{
    [Solution(2023,14,1)]
    [Test(136, testData)]
    public long Part1(CharGrid2 input)
    {
        init(input);

        MoveAll(Point2.North);        
        
        return score;
    }

    void init(CharGrid2 input)
    {
        grid = input;
        score = input.Points.Sum(p => input[p] == 'O' ? input.Height - p.y : 0 );        
    }

    CharGrid2 grid;
    long score;
    long hash;

    private void MoveSingle(Point2 p, Point2 direction)
    {
        var above = p + direction;

        if (grid.Contains(above) && grid[above] == '.' && grid[p] == 'O')            
        {
            grid[above] = 'O';
            grid[p] = '.';
            score += p.y - above.y;
            hash += (above.x * above.y);
            hash -= (p.x * p.y);

            MoveSingle(above, direction);
        }
    }

    private void MoveAll(Point2 direction)
    {
        var crossDirection = new Point2(direction.y, direction.x);
        var rowStart = direction.x + direction.y < 0 ? new Point2(0,0) : new Point2(grid.Width - 1, grid.Height - 1);

        while (grid.Contains(rowStart))
        {
            var p = rowStart;
            while (grid.Contains(p))
            {
                MoveSingle(p, direction);
                p -= crossDirection;
            }
            rowStart -= direction;
        }
    }

    private string getHash(CharGrid2 grid)
    {
        return grid.Points.Sum(p => grid[p] == 'O' ? p.x * p.y : 0).ToString();
    }

    [Solution(2023,14,2)]
    [Test(64, testData)]
    public long Part2(CharGrid2 input)
    {
        init(input);

        var lastScores = new List<long>();
        var lastValues = new List<long>();
        var iterations = 0;

        while (true)
        {
            MoveAll(Point2.North);
            MoveAll(Point2.West);
            MoveAll(Point2.South);
            MoveAll(Point2.East);
            iterations++;
            
            if (lastValues.Contains(hash)) break;
            lastScores.Add(score);
            lastValues.Add(hash);
        }

        // abcde FGHI FGHI FGHI FGH
        // cyclelength = 9 - 5 = 4
        // cycleOffset = 9 - 4 = 5
        // offset = (20 - 5) % 4 = 15 % 4 = 3

        var cycleLength = lastScores.Count - lastValues.IndexOf(hash);
        var cycleOffset = lastScores.Count - cycleLength;
        var offset = (1000000000 - cycleOffset) % cycleLength;
        return lastScores[offset + cycleOffset - 1];

    }


    private const string testData = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....
";
}