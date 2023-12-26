namespace AdventOfCode.AoC2023;

public class Day18
{

    [Solution(2023,18,1)]
    [Test(62, testData)]
    public long Part1(string[] input)
    {
        var turns = string.Join("", input.Select(i => i.First())).Pairwise((f,s) => f.ToString() + s);
        var rightTurns = turns.Count(s => s == "UR" || s == "RD" || s == "DL" || s == "LU");
        bool rightLoop = rightTurns > turns.Count() / 2;

        var map = new Dictionary<Point2, string>();
        var directions = new Dictionary<char,Point2>() {
            ['U'] = Point2.North,
            ['D'] = Point2.South,
            ['R'] = Point2.East,
            ['L'] = Point2.West
        };

        var current = new Point2(0,0);
        

        foreach (var instruction in input)
        {
            var parts = instruction.Split(' ');
            var direction = directions[parts[0][0]];
            var distance = int.Parse(parts[1]);
            var color = parts[2];
            
            for (int i = 0; i < distance; i++)
            {
                current += direction;
                map[current] = color;
            }
        }
        
        var insidePoint = findInternalPoint();
        floodFill(insidePoint);
        return map.Count;


        Point2 findInternalPoint()
        {
            var current = new Point2(0,0);

            foreach (var instruction in input)
            {
                var parts = instruction.Split(' ');
                var direction = directions[parts[0][0]];
                var distance = int.Parse(parts[1]);

                var internalDirection = rightLoop? direction.Clockwise() : direction.Anticlockwise();

                for (int i = 0; i < distance; i++)
                {
                    current += direction;
                    if (!map.ContainsKey(current + internalDirection))
                    {
                        return current + internalDirection;
                    }                    
                }
            }

            throw new InvalidOperationException();
        }


        void floodFill(Point2 point)
        {
            var queue = new Queue<Point2>();
            queue.Enqueue(point);
            map[point] = NO_COLOR;

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                foreach (var d in Point2.CompassDirections)
                {
                    var n = d + next;
                    if (!map.ContainsKey(n))
                    {
                        map[n] = NO_COLOR;
                        queue.Enqueue(n);
                    }
                }
            }
        }

    }

    [Solution(2023,18,2)]
    [Test(952408144115, testData)]
    public long Part2(string[] input)
    {
        var turns = string.Join("", input.Select(i => i.First())).Pairwise((f,s) => f.ToString() + s);
        var rightTurns = turns.Count(s => s == "UR" || s == "RD" || s == "DL" || s == "LU");
        bool rightLoop = rightTurns > turns.Count() / 2;

        var directions = new Point2[] { Point2.East, Point2.South, Point2.West, Point2.North };

        var current = new Point2(0,0);        

        var boundaries = new HashSet<Rect>();
        Point2? startingLocation = null;
        var boundaryLength = 0L;

        foreach (var instruction in input)
        {
            var color = instruction.Split(' ')[2];
            var direction = directions[int.Parse(color.Substring(7,1))];
            var distance = int.Parse(color.Substring(2,5), System.Globalization.NumberStyles.HexNumber);
            var end = current + distance * direction;

            if (startingLocation == null)
            {
                startingLocation = current + direction * 2 + (rightLoop ? direction.Clockwise() : direction.Anticlockwise()) * 2;
            }

            var edge = new Rect(current, end);

            boundaries.Add(new Rect(edge.topLeft, edge.bottomRight + new Point2(1,1)));
            current = end;
            boundaryLength += distance;
        }

        var xs = boundaries.Select( t => t.topLeft.x).Union(boundaries.Select( t => t.bottomRight.x)).Distinct().OrderBy(x => x).ToArray();
        var ys = boundaries.Select( t => t.topLeft.y).Union(boundaries.Select( t => t.bottomRight.y)).Distinct().OrderBy(y => y).ToArray();

        var x1 = Array.IndexOf(xs, xs.Last(x => x < startingLocation.Value.x));
        var y1 = Array.IndexOf(ys, ys.Last(y => y < startingLocation.Value.y));


        var queue = new Queue<Point2>();
        queue.Enqueue(new Point2(x1,y1));
        var contained = 0L;
        var visited = new HashSet<Point2>();
        visited.Add(queue.Peek());

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            var rect = new Rect(new Point2(xs[next.x], ys[next.y]), new Point2(xs[next.x + 1], ys[next.y + 1]));
            contained += rect.Area;

            var adjRects = new[] { next + Point2.West, next + Point2.East, next + Point2.North, next + Point2.South };
            var adjEdges = new[] { rect.LeftEdge, rect.RightEdge, rect.TopEdge, rect.BottomEdge};

            for (int i = 0; i < 4; i++)
            {
                if (!visited.Contains(adjRects[i]) && !boundaries.Any(b => b.Contains(adjEdges[i])))
                {
                    visited.Add(adjRects[i]);
                    queue.Enqueue(adjRects[i]);
                }

            }
        }

        return contained + boundaryLength;
    }

    private const string NO_COLOR = "";

    private const string testData = @"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
";
}