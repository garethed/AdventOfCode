using System.Text;

namespace AdventOfCode.AoC2015;

public class Day10
{

    const string inputData = "3113322113";

    [Solution(2015,10,1)]
    public int Part1() => iterate(inputData, 40).Length;

    [Solution(2015,10,2)]
    public int Part2() => iterate(inputData, 50).Length;


    [Test("312211", "1", 5)]
    public string iterate(string input, int rounds)
    {
        var data = input.Select(c => int.Parse(c.ToString())).ToList();

        for (var i = 0; i < rounds; i++)
        {
            data = expand(data);
        }

        return string.Join("", data.Select(i => i.ToString()));
    }

    private List<int> expand(List<int> input)
    {
        var output = new List<int>();
        var prev = input[0];
        var count = 0;

        foreach (var i in input)
        {
            if (i == prev) 
            {
                count++;
            }
            else
            {
                output.Add(count);
                output.Add(prev);
                prev = i;
                count = 1;
            }

        }

        output.Add(count);
        output.Add(prev);

        return output;
    }
}