namespace AdventOfCode.AoC2023;

class Day6
{
    [Solution(2023,6,1)]
    [Test(288, testData)]
    public long Part1(string[] input)
    {
        long[][] data = input.Select(l => l.After(":").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(i => long.Parse(i)).ToArray()).ToArray();

        var margin = 1;

        for (long i = 0; i < data[0].Length; i++)
        {
            var totalTime = data[0][i];
            var targetDistance = data[1][i];
            var combinations = 0;
            
            for (long t = 1; t < totalTime; t++)
            {
                if (distance(t, totalTime) > targetDistance) combinations++;
            }

            margin *= combinations;
        }

        return margin;
    }


    [Solution(2023,6,2)]
    [Test(71503, testData)]
    public long Part2(string[] input)
    {
        return Part1(input.Select(i => i.Replace(" ", "")).ToArray());
    }    

    long distance(long buttonTime, long totalTime)
    {
        return buttonTime * (totalTime - buttonTime);
    }

private const string testData = @"Time:      7  15   30
    Distance:  9  40  200";

}