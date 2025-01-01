using System.Drawing;

namespace AdventOfCode.AoC2024;

class Day18
{
    [Solution(additionalInputs = [70, 1024])]
    [Test(22, testInput, 6, 12)]
    public int Part1(Point2[] points, int max, int limit)
    {    
        var blocked = points.Take(limit).ToHashSet();
        var times = new Dictionary<Point2, int>();
        var queue = new Queue<Point2>();
        var target = new Point2(max, max);
        var start = new Point2(0,0);
        var rect = new Rect(0, 0, max + 1, max + 1);

        queue.Enqueue(start);
        times[start] = 0;

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var time = times[next];
            
            foreach (var neighbour in next.Neighbours4)
            {
                if (times.ContainsKey(neighbour))
                {
                    continue;
                }
                else if (neighbour == target)
                {
                    return time + 1;
                }
                else if (rect.Contains(neighbour) && !blocked.Contains(neighbour))
                {
                    times[neighbour] = time + 1;
                    queue.Enqueue(neighbour);
                }                                            
            }
        }

        return -1;
    }

    [Solution(additionalInputs = [70])]
    [Test(new int[] {6,1}, testInput, 6)]
    public int[] Part2(Point2[] points, int max)
    {
        var i = 1;

        while (true)
        {
            if (Part1(points, max, i) == -1)
            {
                return [points[i - 1].x, points[i - 1].y];
            }

            i++;
        }
    }


    private const string testInput = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";
}
