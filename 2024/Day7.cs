namespace AdventOfCode.AoC2024;

class Day7
{
    [Solution]
    [Test(3749, testData)]
    public long Part1(Calibration[] input)
    {
        return input
        .Where(i => i.IsValid([(long a, long b) => a * b, (a,b) => a + b]))
        .Sum(i => i.Value);
    }

    [Solution]
    [Test(11387, testData)]
    public long Part2(Calibration[] input)
    {
        var append  = (long a, long b) => long.Parse(a.ToString() + b.ToString());

        return input
        .Where(i => i.IsValid([(long a, long b) => a * b, (a,b) => a + b, append]))
        .Sum(i => i.Value);
    }    

    public class Calibration
    {
        public long Value;
        public long[] Elements;

        public Calibration(string input)
        {
            var parts = input.Split(": ");
            Value = long.Parse(parts[0]);
            Elements = parts[1].Split(" ").Select(i => long.Parse(i)).ToArray();
        }

        public bool IsValid(Func<long, long, long>[] operators)
        {
            return IsValid(Elements[0], 1);

            bool IsValid(long totalSoFar, long i)
            {
                if (totalSoFar > Value) return false;
                if (i == Elements.Length) return totalSoFar == Value;
                return operators.Any(op => IsValid(op(totalSoFar, Elements[i]), i + 1));
            }
        }
    }

    private const string testData = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
";
}