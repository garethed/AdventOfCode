using System.Dynamic;

namespace AdventOfCode.AoC2023;

public class Day16
{
    [Solution(2023,16,1)]
    [Test(46, testData)]
    public int Part1(CharGrid2 input)
    {
        return CountEnergised(input, new Point2(0,0), Point2.East);
    }


    private int CountEnergised(CharGrid2 input, Point2 start, Point2 direction)
    {
        var energised = new HashSet<Point2>();
        var previousSteps = new HashSet<Tuple<Point2, Point2>>();
        propagateBeam(start, direction);

        void propagateBeam(Point2 p, Point2 direction)
        {
            while (input.Contains(p))
            {
                if (!previousSteps.Add(Tuple.Create(p, direction))) return;

                energised.Add(p);            
                switch (input[p])
                {
                    case '\\':
                        direction = new Point2(direction.y, direction.x);
                        break;
                    case '/':
                        direction = new Point2(-direction.y, -direction.x);
                        break;
                    case '|':
                        if (direction == Point2.East || direction == Point2.West)
                        {
                            propagateBeam(p + Point2.North, Point2.North);
                            direction = Point2.South;
                        }
                        break;
                    case '-':
                        if (direction == Point2.North || direction == Point2.South)
                        {
                            propagateBeam(p + Point2.East, Point2.East);
                            direction = Point2.West;
                        }
                        break;                    
                }
                p += direction;
            }
        }

        return energised.Count();
    }

    [Solution(2023,16,2)]
    [Test(51, testData)]
    public long Part2(CharGrid2 input)
    {
        var xs = Enumerable.Range(0, input.Width);
        var ys = Enumerable.Range(0, input.Height);

        var startingPositions = new List<Tuple<Point2, Point2>>();

        startingPositions.AddRange(xs.Select(x => Tuple.Create(new Point2(x, 0), Point2.South)));
        startingPositions.AddRange(xs.Select(x => Tuple.Create(new Point2(x, input.Height - 1), Point2.North)));
        startingPositions.AddRange(ys.Select(y => Tuple.Create(new Point2(0, y), Point2.East)));
        startingPositions.AddRange(ys.Select(y => Tuple.Create(new Point2(input.Width - 1, y), Point2.West)));

        return startingPositions.Max(s => CountEnergised(input, s.Item1, s.Item2));
    }

    private const string testData = @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....
";

}