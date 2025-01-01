namespace AdventOfCode.AoC2024;

class Day1 
{
    [Solution(2024,1,1)]
    [Test(11, testData)]
    public int Part1(string[] input)
    {
        var fst = new List<int>();
        var snd = new List<int>();

        foreach (var line in input)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            fst.Add(int.Parse(parts[0]));
            snd.Add(int.Parse(parts[1]));
        }

        return fst.OrderBy(x => x)
            .Zip(snd.OrderBy(x => x))
            .Sum(x => Math.Abs(x.First - x.Second));

    }

    [Solution(2024,1,2)]
    [Test(31, testData)]
    public int Part2(string[] input)
    {
        var fst = new List<int>();
        var snd = new List<int>();

        foreach (var line in input)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            fst.Add(int.Parse(parts[0]));
            snd.Add(int.Parse(parts[1]));
        }

        return fst.Sum(x => x * snd.Count(y => y == x));        
    }

    private const string testData = @"3   4
4   3
2   5
1   3
3   9
3   3";
    
}
