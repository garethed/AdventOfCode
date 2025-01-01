namespace AdventOfCode.AoC2024;

class Day10
{
    [Solution]
    [Test(36, testData)]
    public long Part1(IntGrid2 data)
    {
        var peaks = data.Where(kv => kv.Value == 9).Select(kv => kv.Key);
        var reachable = new SetValuedDictionary<Point2, Point2>();
        peaks.ForEach(p => reachable.Add(p, p));

        var queue = new Queue<Point2>(peaks);        

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var height = data[next];
            foreach (var neighbour in data.Neighbours4(next))
            {
                if (data[neighbour] == height - 1)
                {
                    reachable[neighbour].UnionWith(reachable[next]);
                    queue.Enqueue(neighbour);
                }
            }
        }

        return data.Where(kv => kv.Value == 0).Sum(kv => reachable[kv.Key].Count);
    }

    [Solution]
    [Test(81, testData)]
    public long Part2(IntGrid2 data)
    {
        var peaks = data.Where(kv => kv.Value == 9).Select(kv => kv.Key);
        var reachable = new ListValuedDictionary<Point2, Point2>();
        peaks.ForEach(p => reachable.Add(p, p));

        var queue = new Queue<Point2>(peaks);        

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var height = data[next];
            foreach (var neighbour in data.Neighbours4(next))
            {
                if (data[neighbour] == height - 1)
                {
                    reachable[neighbour].AddRange(reachable[next]);
                    if (!queue.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        return data.Where(kv => kv.Value == 0).Sum(kv => reachable[kv.Key].Count);
    }    

    private const string testData = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
";
}