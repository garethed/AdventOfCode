using System.Dynamic;

namespace AdventOfCode.AoC2023;

class Day5
{
    [Solution(2023,5,1)]
    [Test(35, testData)]
    public long Part1(string input)
    {
        var sections = input.Split("\n\n");
        var seeds = sections[0].After(": ").Split(" ").Select(n => long.Parse(n)).ToList();

        var map = SeedMap.buildMap(sections, 1);

        return seeds.Min(s => map.mapValue(s));

    }

    [Solution(2023,5,2)]
    [Test(46, testData)]
    public long Part2(string input)
    {
        var sections = input.Split("\n\n");
        var seeds = sections[0].After(": ").Split(" ").Select(n => long.Parse(n)).ToList();

        var map = SeedMap.buildMap(sections, 1);

        var seedRanges = seedsToRanges(seeds).ToList();

        while (map != null)
        {
            seedRanges = seedRanges.SelectMany(sr => map.mapRange(sr)).ToList();
            map = map.nextMap;
        }

        return seedRanges.Min(r => r.from);

        IEnumerable<Range> seedsToRanges(List<long> seeds)
        {
            for (int i = 0; i < seeds.Count / 2; i++)
            {
                yield return new Range(seeds[i * 2], seeds[i * 2] + seeds[i * 2 + 1] - 1);
            }
        }

    }    

    record Range(long from, long to)
    {
        public IEnumerable<Range> intersect(long splitPoint)
        {
            if (splitPoint > from && splitPoint <= to)
            {
                yield return new Range(from, splitPoint - 1);
                yield return new Range(splitPoint, to);
            }
            else 
            {
                yield return this;
            }
        }
        
    }

    class SeedMap
    {
        public SortedSet<MapRange> ranges = new SortedSet<MapRange>();
        public SeedMap? nextMap;   

        long mapValueOnce(long value)     
        {
            foreach (var r in ranges)
            {
                if (r.Maps(value)) return r.Map(value);
                if (r.SourceFrom > value) return value;
            }
            return value;
        }

        public long mapValue(long value)
        {
            var nextValue = mapValueOnce(value);
            return nextMap == null ? nextValue : nextMap.mapValue(nextValue);
        }

        public List<Range> mapRange(Range range)
        {
            var splitRange = new List<Range>() { range };

            foreach (var mr in ranges)
            {
                splitRange = splitRange.SelectMany(r => r.intersect(mr.SourceFrom)).ToList();
                splitRange = splitRange.SelectMany(r => r.intersect(mr.SourceTo + 1)).ToList();
            }

            return splitRange.Select(r => new Range(mapValueOnce(r.from), mapValueOnce(r.to))).ToList();
        }    

        public static SeedMap buildMap(string[] sections, long sectionId)
        {
            var lines = sections[sectionId].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1);
            var map = new SeedMap();

            foreach (var l in lines)
            {
                var data = l.ToLongArray();
                map.ranges.Add(new MapRange(data[0], data[1], data[2]));
            }

            if (sectionId < sections.Length - 1)                                            
            {
                map.nextMap = buildMap(sections, sectionId + 1);
            }

            return map;
        }
    }

    record MapRange(long DestinationFrom, long SourceFrom, long Length) : IComparable<MapRange>
    {
        public bool Maps(long value) => SourceFrom <= value && (SourceFrom + Length) > value;
        public long Map(long value) => value - SourceFrom + DestinationFrom;

        public long SourceTo => SourceFrom + Length - 1;

        public int CompareTo(MapRange? other)
        {
            return SourceFrom.CompareTo(other.SourceFrom);
        }
    }

    private const string testData = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";
}