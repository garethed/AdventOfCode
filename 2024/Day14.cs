using System.Drawing;
using System.Formats.Asn1;

namespace AdventOfCode.AoC2024;

class Day14
{
    [Solution(additionalInputs = [101, 103])]
    [Test(12, testInput, 11, 7)]
    public long Part1(List<Robot> robots, int width, int height)
    {
        var midx = width / 2;
        var midy = height / 2;
        Rect[] quads = [new Rect(0, 0, midx, midy), new Rect(midx + 1, 0, midx, midy), new Rect(midx + 1, midy + 1, midx , midy), new Rect(0, midy + 1, midx, midy)];

        var movedRobots = robots.Select(r => (r.p + 100 * r.d).Mod(width, height)).ToList();

        var counts = quads
            .Select(q => movedRobots.Count(r => q.Contains(r)));
        
        return counts.Aggregate(1, (a,b) => a * b);
    }


    [Solution(additionalInputs = [101, 103])]
    public long Part2(List<Robot> robots, int width, int height)
    {
        var gridSize = 5;

        int[,] grid = new int[gridSize, gridSize];

        var steps = 0;
        var maxScore = 0L;
        var maxSteps = 0;
        var maxPattern = new HashSet<Point2>();

        while (steps < 100000)
        {
            steps++;
            var movedRobots = robots.Select(r => (r.p + steps * r.d).Mod(width, height)).ToHashSet();

            var bucketed = movedRobots
                .GroupBy(r => ((r.x - 50) / gridSize, r.y / gridSize))
                .Aggregate(1L, (t, g) => g.Count() == 0 ? t : t + g.Count() * g.Count());

            if (bucketed < 0) throw new Exception();


            if (bucketed > maxScore)
            {
                maxScore = bucketed;
                maxPattern = movedRobots;
                maxSteps = steps;
            }
        }

        //Print(maxPattern, maxSteps);
        return maxSteps;

        void Print(HashSet<Point2> points, int steps)
        {            
            Console.WriteLine();
            Console.WriteLine(steps);
            Console.WriteLine();

            for (int y = 0; y < height; y++)
            {
                Console.WriteLine();
                for (int x = 0; x < width; x++)
                {
                    if (points.Contains(new Point2(x, y)))
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
        }
    }    


    [RegexDeserializable(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
    public record Robot(int px, int py, int dx, int dy)
    {
        public Point2 p = new Point2(px, py);
        public Point2 d = new Point2(dx, dy);
    }


    private const string testInput = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";
}