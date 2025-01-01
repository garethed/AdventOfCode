using System.Text.RegularExpressions;

namespace AdventOfCode.AoC2024;

class Day3
{
    [Solution(2024,3,1)]
    [Test(161, "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))")]
    public int Part1(string input)
    {
        var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
        return matches.Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }

    [Solution(2024,3,2)]
    [Test(48, "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))")]
    public int Part2(string input)
    {
        var muls = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
        var dos = Regex.Matches(input, @"do(n\'t)?\(\)");
        return muls.Sum(m => {
            var prev = dos.LastOrDefault(d => d.Index < m.Index);
            if (prev == null || prev.Value == "do()")
            {
                return int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
            }
            else return 0;
        });
    }
}