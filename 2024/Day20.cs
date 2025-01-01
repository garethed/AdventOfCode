namespace AdventOfCode.AoC2024;

class Day20
{
    [Solution (additionalInputs = [2, 100])]
    [Solution (part = 2, additionalInputs = [20, 100])]
    [Test(8, testInput, 2, 12)]
    [Test(41, testInput, 20, 70)]
    public long Part1(CharGrid2 map, int maxCheat, int targetSaving)
    {
        var start = map.First(START);
        var end = map.First(END);
        var fromStart = buildCostMap(start);
        var fromEnd = buildCostMap(end);
        var normalCost = fromStart[end];

        var shortcuts = 0;

        foreach (var kv in fromStart)
        {
            var costSoFar = kv.Value;
            var p = kv.Key;
            if (costSoFar < normalCost - 2)
            {
                for (int x = -maxCheat; x <= maxCheat; x++)
                {
                    var maxy = maxCheat - Math.Abs(x);
                    for (int y = -maxy; y <= maxy; y++)
                    {
                        var shortcut = p + new Point2(x,y);
                        if (map.Contains(shortcut) && map[shortcut] != WALL)
                        {
                            var newCost = costSoFar + Math.Abs(x) + Math.Abs(y) + fromEnd[shortcut];
                            if (normalCost - newCost >= targetSaving)
                            {
                                shortcuts++;
                            }
                        }                        
                    }
                }
            }
        }

        return shortcuts;


        Dictionary<Point2, int> buildCostMap(Point2 start)
        {
            var costs = new Dictionary<Point2, int>();
            costs[start] = 0;
            var queue = new Queue<Point2>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var p = queue.Dequeue();
                var c = costs[p];

                foreach (var n in map.Neighbours4(p))
                {
                    if (!costs.ContainsKey(n) && map[n] != '#')
                    {
                        costs[n] = c + 1;
                        queue.Enqueue(n);
                    }
                }
            }

            return costs;
        }
    }


    const char START = 'S';
    const char END = 'E';
    const char WALL = '#';


    private const string testInput = @"###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############";
}
