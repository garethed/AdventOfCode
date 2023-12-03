using System.Security.Cryptography;

namespace AdventOfCode.AoC2015;

public class Day4
{
    private string defaultInput = "ckczppom";

    [Solution(2015,4,1)]
    public int Part1() => FindKey(defaultInput, 5);

    [Solution(2015,4,2)]
    public int Part2() => FindKey(defaultInput, 6);


    [Test(609043, "abcdef", 5)]
    public int FindKey(string input, int count)
    {
        var match = new String('0', count);
        var current = 1;
        var key = input + current.ToString();

        while (!Utils.MD5(key).StartsWith(match))
        {
            current++;
            key = input + current.ToString();
        }

        return current;
    }
}