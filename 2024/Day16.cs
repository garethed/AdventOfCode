namespace AdventOfCode.AoC2024;

class Day16
{
    [Solution()]
    [Test(11048, testInput)]
    public long Part1(CharGrid2 map)
    {
        var start = new Pose(map.First('S'), Point2.East);
        var endp = map.First('E');
        var ends = new[] {new Pose(endp, Point2.North), new Pose(endp, Point2.East)};
        var costs = new Dictionary<Pose, int>();

        var queue = new Queue<Pose>();
        queue.Enqueue(start);
        costs[start] = 0;

        while (queue.Any())
        {
            var p = queue.Dequeue();
            var cost = costs[p];
            foreach (var d in Point2.CompassDirections)
            {
                if (d != -1 * p.d
                    && map.Contains(p.p + d) 
                    && map[p.p + d] != WALL)
                {
                    var newCost =  cost + (d == p.d ? 1 : 1001);
                    var newp = new Pose(p.p + d, d);
                    if (costs.GetValueOrDefault(newp, int.MaxValue) > newCost)
                    {
                        costs[newp] = newCost;
                        queue.Enqueue(newp);
                    }
                }
            }

        }

        return ends.Min(e => costs.GetValueOrDefault(e, int.MaxValue));
    }

    [Solution()]
    [Test(64, testInput)]
    public long Part2(CharGrid2 map)
    {
        var start = new Pose(map.First('S'), Point2.East);
        var endp = map.First('E');
        var ends = new[] {new Pose(endp, Point2.North), new Pose(endp, Point2.East)};
        var costs = new Dictionary<Pose, int>();

        var queue = new Queue<Pose>();
        queue.Enqueue(start);
        costs[start] = 0;

        while (queue.Any())
        {
            var p = queue.Dequeue();
            var cost = costs[p];
            foreach (var d in Point2.CompassDirections)
            {
                if (d != -1 * p.d
                    && map.Contains(p.p + d) 
                    && map[p.p + d] != WALL)
                {
                    var newCost =  cost + (d == p.d ? 1 : 1001);
                    var newp = new Pose(p.p + d, d);
                    if (costs.GetValueOrDefault(newp, int.MaxValue) > newCost)
                    {
                        costs[newp] = newCost;
                        queue.Enqueue(newp);
                    }
                }
            }

        }

        var end = ends.MinBy(e => costs.GetValueOrDefault(e, int.MaxValue));

        var pointsOnRoute = new HashSet<Pose>();

        addRoutes(end);
        return pointsOnRoute.Select(p => p.p).Distinct().Count();

        void addRoutes(Pose p)
        {
            pointsOnRoute.Add(p);

            foreach (var d in Point2.CompassDirections)
            {
                foreach (var p2 in Point2.CompassDirections.Select(d2 => new Pose(p.p - d, d2)))
                {
                    if (costs.ContainsKey(p2))
                    {
                        var diff = costs[p] - costs[p2];
                        if (diff == 1 || diff == 1001)
                        {
                            addRoutes(p2);
                        }
                    }
                }
            }
        }


        


    }


    const char WALL = '#';

    const string testInput = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";
}
