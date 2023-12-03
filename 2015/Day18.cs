
namespace AdventOfCode.AoC2015;

class Day18
{
    [Solution(2015,18,1,100)]
    [Test(4, testData, 4)]
    public int Part1(string[] input, int iterations)
    {
        var current = createGrid(input);
        var next = (bool[,]) current.Clone();

        for (var i = 0; i < iterations; i++)
        {
            evolveGrid(current, next);
            (current, next) = (next,current);
        }

        return current.Cast<bool>().Count(b => b);        
    }

    [Solution(2015,18,2,100)]
    [Test(17, testData, 5)]
    public int Part2(string[] input, int iterations)
    {
        var current = createGrid(input);
        var next = (bool[,]) current.Clone();
        var w = current.GetLength(0);
        var h = current.GetLength(1);

        for (var i = 0; i < iterations; i++)
        {
            current[1, 1] = true;
            current[1, h - 2] = true;
            current[w - 2, 1] = true;
            current[w - 2, h - 2] = true;

            evolveGrid(current, next);            
            (current, next) = (next,current);
        }

        current[1, 1] = true;
        current[1, h - 2] = true;
        current[w - 2, 1] = true;
        current[w - 2, h - 2] = true;


        return current.Cast<bool>().Count(b => b);        
    }    

    private void evolveGrid(bool[,] current, bool[,] next)
    {
        for (int y = 1; y < current.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < current.GetLength(0) - 1; x++) 
            {
                var alive = current[x,y];
                var count = new Point2(x,y).Neighbours8.Count(n => current[n.x, n.y]);

                next[x,y] = (alive && (count == 2 || count == 3)) || (!alive && count == 3);
            }
        }                
    }

    private bool[,] createGrid(string[] input)
    {
        var w = input[0].Length;
        var h = input.Length;
        var ret = new bool[w + 2,h + 2];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++) 
            {
                ret[x + 1, y + 1] = input[y][x] == '#';
            }
        }

        return ret;
    }

    const string testData = @".#.#.#
...##.
#....#
..#...
#.#..#
####..";

}