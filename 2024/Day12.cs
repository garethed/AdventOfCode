using System.Drawing;
using System.Security.Cryptography;

namespace AdventOfCode.AoC2024;

class Day12
{
    [Solution()]
    [Test(1930, testInput)]
    public long Part1(CharGrid2 input)
    {
        var nextRegionId = 0;
        var plotsRemaining = new HashSet<Point2>(input.Points);

        var total = 0;

        while (plotsRemaining.Any())
        {
            var perimeter = 0;
            var area = 0;
            var start = plotsRemaining.First();
            var regionId = input[start];

            ExpandPlot(start);
            total += area * perimeter;

            void ExpandPlot(Point2 neighbour)
            {
                if (!input.Contains(neighbour) || input[neighbour] != regionId)
                {
                    perimeter++;
                }
                else if (plotsRemaining.Contains(neighbour))
                {
                    area++;
                    plotsRemaining.Remove(neighbour);
                    foreach (var next in neighbour.Neighbours4)
                    {
                        ExpandPlot(next);
                    }
                }

            }
        }

        return total;

    }

    [Solution()]
    [Test(368L, testInput2)]
    [Test(1206L, testInput)]
    public long Part2(CharGrid2 input)
    {
        var nextRegionId = 0;
        var plotsRemaining = new HashSet<Point2>(input.Points);

        var total = 0;

        while (plotsRemaining.Any())
        {
            var area = 0;
            var perimeter = 0;
            var start = plotsRemaining.First();
            var regionId = input[start];
            var edges = new HashSet<(Point2 p1, Point2 p2)>();

            ExpandPlot(start, new Point2(-1000, -1000));

            while (edges.Any())
            {
                var edge = edges.First();
                edges.Remove(edge);
                removeAdjacent(edge);
                perimeter++;

                void removeAdjacent((Point2 p1, Point2 p2) e1)
                {
                    var adjacent = edges.Where(e2 => (e2.p1 - e1.p1) == (e2.p2 - e1.p2) 
                    && ((e2.p1 - e1.p1) == (e1.p1 - e1.p2).Clockwise() || (e2.p1 - e1.p1) == (e1.p1 - e1.p2).Anticlockwise()));
                    foreach(var edge in adjacent)
                    {
                        edges.Remove(edge);
                        removeAdjacent(edge);
                    }                    
                }

            }

            total += area * perimeter;

            void ExpandPlot(Point2 neighbour, Point2 prev)
            {
                if (!input.Contains(neighbour) || input[neighbour] != regionId)
                {
                    edges.Add((neighbour, prev));                        
                }
                else if (plotsRemaining.Contains(neighbour))
                {
                    area++;
                    plotsRemaining.Remove(neighbour);
                    foreach (var next in neighbour.Neighbours4)
                    {
                        ExpandPlot(next, neighbour);
                    }
                }

            }
        }

        return total;        
    }


    private const string testInput = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
";

private const string testInput2 = @"AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA
";
}