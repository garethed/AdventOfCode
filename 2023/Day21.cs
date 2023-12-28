using System.Data.Common;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace AdventOfCode.AoC2023;

public class Day21
{
    [Solution(2023,21,1, 64)]
    [Test(16, testData, 6)]
    public long Part1(CharGrid2 input, int targetSteps)
    {
        return reachablePlots(input, targetSteps, input.Points.First(p => input[p] == 'S'));        
    }

    long reachablePlots(CharGrid2 input, int targetSteps, Point2 startLocation)
    {
        var visited = new HashSet<Point2>();
        var queue = new Queue<Tuple<Point2,int>>();
        var targetCount = 0;

        queue.Enqueue(Tuple.Create(startLocation,0));

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var p = next.Item1;
            var d = next.Item2 + 1;
            foreach (var n in input.Neighbours4(p))
            {
                if (input[n] == '.' || input[n] == 'S') 
                {
                    if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        queue.Enqueue(Tuple.Create(n, d));
                        if (d <= targetSteps && d % 2 == targetSteps % 2)
                        {
                            targetCount++;
                        }
                    }
                }
            }            
        }        
        return targetCount;
    }

    [Solution(2023,21,2, 26501365)]
    public long Part2(CharGrid2 input, int targetSteps)
    {
        var totalReachable = 0L;
        var maxPlots = (targetSteps - 65) / 131 + 2; // 202 300 exactly
        var reachableFromEvenParityTiles = reachablePlots(input, 199, new Point2(65,65));
        var reachableFromOddParityTiles = reachablePlots(input, 200, new Point2(65,65));
        var start = new Point2(65,65);
        var memoised = new Dictionary<Tuple<int,Point2>,long>();
        
        for (int ix = -maxPlots; ix <= maxPlots; ix++)
        {            
            var ymax = maxPlots - Math.Abs(ix);
            if (ix % 10000 == 0) Utils.WriteTransient(ix.ToString());

            for (int iy = -ymax; iy <= ymax; iy++)
            {                
                if (ymax - Math.Abs(iy) == 3)
                {
                    var numberToSkip = 2 * Math.Abs(iy) + 1;
                    
                    totalReachable += (numberToSkip - 1) / 2 * (reachableFromEvenParityTiles + reachableFromOddParityTiles);
                    totalReachable += (ix + iy) % 2 == 0 ? reachableFromEvenParityTiles : reachableFromOddParityTiles;
                    iy = -iy;
                    continue;
                }

                var closestPoint = (131 * new Point2(ix, iy)) - (65 * new Point2(Math.Sign(ix), Math.Sign(iy)));

                if (closestPoint.Abs() <= targetSteps)
                {
                    var closestPointTileCoord = start - (65 * new Point2(Math.Sign(ix), Math.Sign(iy)));
                    
                    // closstPoint is in coord system where S is 0,0 so this is right
                    totalReachable += reachablePlotsMemoised(targetSteps - closestPoint.Abs(), closestPointTileCoord);

                }
            }
        }

        return totalReachable;

        long reachablePlotsMemoised(int targetSteps, Point2 startLocation)
        {
            var t = Tuple.Create(targetSteps, startLocation);
            if (!memoised.ContainsKey(t))
            {
                var count = reachablePlots(input, targetSteps, startLocation);
                memoised[t] = count;
            }
            return memoised[t];
        }
    }


    private const string testData = @"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........
";
}