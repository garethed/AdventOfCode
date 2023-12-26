using System.Drawing;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace AdventOfCode.AoC2023;

public class Day10
{
    private HashSet<Point2> loopPoints;
    private Point2 start;
    private Point2 startDirection;

    [Solution(2023,10,1)]
    [Test(8, testData)]
    public long Part1(CharGrid2 grid)
    {
        start = grid.Points.Where(p => grid[p] == 'S').First();
        /*var direction = Point2.North;
        if (!grid.Contains(start + direction) || !"7|F".Contains(grid[start + direction])) direction = Point2.South;
        if (!grid.Contains(start + direction) || !"J|L".Contains(grid[start + direction])) direction = Point2.East;*/

        foreach (var startDirection in new Point2(0,0).Neighbours4)
        {
            this.startDirection = startDirection;
            try
            {
                loopPoints = new HashSet<Point2>();
                loopPoints.Add(start);

                var direction = startDirection;
                var pos = start + direction;
                var steps = 1;
                while (pos != start)
                {
                    loopPoints.Add(pos);
                    direction = getNextDirection(grid, pos, direction);
                    pos = pos + direction;
                    steps++;                    
                }
                return (steps + 1) / 2;
            }
            catch (InvalidOperationException)
            {}
        }

        return -1;
                
    }

    [Solution(2023,10,2)]
    [Test(101, testData2)]
    public int Part2(CharGrid2 grid)
    {
        Part1(grid);

        var rightPoints = new HashSet<Point2>();
        var leftPoints = new HashSet<Point2>();

        var direction = startDirection;
        var pos = start + direction;

        while (pos != start)
        {
            var nextDirection = getNextDirection(grid, pos, direction);
            if (nextDirection == direction)
            {
                rightPoints.Add(pos + direction.Clockwise());   
                leftPoints.Add(pos + direction.Anticlockwise());                
            }
            else if (nextDirection == direction.Anticlockwise())
            {
                rightPoints.Add(pos + direction.Clockwise());
                rightPoints.Add(pos + direction);
            }
            else if (nextDirection == direction.Clockwise())
            {
                leftPoints.Add(pos + direction.Anticlockwise());
                leftPoints.Add(pos + direction);

            }
            pos = pos + nextDirection;
            direction = nextDirection;
        }

        rightPoints.ExceptWith(loopPoints);
        leftPoints.ExceptWith(loopPoints);

        return Math.Max(countInternal(leftPoints), countInternal(rightPoints));

        int countInternal(HashSet<Point2> points)
        {
            while (true)
            {
                var additionalPoints = new HashSet<Point2>();
                additionalPoints.UnionWith(points.SelectMany( p => p.Neighbours4));
                additionalPoints.ExceptWith(points);
                additionalPoints.ExceptWith(loopPoints);
                var beforeGridCheck = additionalPoints.Count();
                additionalPoints.IntersectWith(grid.Points);
                if (additionalPoints.Count != beforeGridCheck) return 0;                

                if (!additionalPoints.Any())
                {
                    return points.Count;
                }
                
                points.UnionWith(additionalPoints);
            }
        }


    }

        Point2 getNextDirection(CharGrid2 grid, Point2 location, Point2 prevDirection)
        {
            if (!grid.Contains(location)) throw new InvalidOperationException();

            return grid[location] switch 
            {
                '|' or '-' => prevDirection,
                'J' or 'F' => new Point2(-prevDirection.y, -prevDirection.x),
                '7' or 'L' => new Point2(prevDirection.y, prevDirection.x),
                _ => throw new InvalidOperationException()

            };
        }    

    public const string testData = @"7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ

";

    public const string testData2 = @"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJIF7FJ-
L---JF-JLJIIIIFJLJJ7
|F|F-JF---7IIIL7L|7|
|FFJF7L7F-JF7IIL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L
";
}