namespace AdventOfCode.AoC2024;

class Day8
{
    [Solution]
    [Test(14, testData)]
    public long Part1(CharGrid2 map)
    {
        var antinodes = new HashSet<Point2>();

        void findAntiNodes((Point2 fst, Point2 snd) input) 
        {
            antinodes.Add(input.fst + (input.fst - input.snd));
            antinodes.Add(input.snd + (input.snd - input.fst));
        };
        
        map.GroupBy( kv => kv.Value, kv => kv.Key)
            .Where(kv => kv.Key != '.')
            .SelectMany(g => g.GetUniquePairs())
            .ForEach(findAntiNodes);

        return map.ContainedPoints(antinodes).Count();
    }

    [Solution]
    [Test(34, testData)]
    public long Part2(CharGrid2 map)
    {
        var antinodes = new HashSet<Point2>();

        void findAntiNodes((Point2 fst, Point2 snd) input) 
        {
            var d = (input.fst - input.snd);
            var gcf = (int) Utils.gcf(Math.Abs(d.x), Math.Abs(d.y));
            d = new Point2(d.x / gcf, d.y / gcf);
            var p = input.fst;

            while (map.Contains(p))
            {
                antinodes.Add(p);
                p += d;
            }

            p = input.fst - d;
            while (map.Contains(p))
            {
                antinodes.Add(p);
                p -= d;
            }            
        };
        
        map.GroupBy( kv => kv.Value, kv => kv.Key)
            .Where(kv => kv.Key != '.')
            .SelectMany(g => g.GetUniquePairs())
            .ForEach(findAntiNodes);
            
        return map.ContainedPoints(antinodes).Count();
    }    

    private const string testData = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
";
}