namespace AdventOfCode.AoC2024;

class Day11
{
    [Solution(additionalInputs = [25])]
    [Solution(part = 2, additionalInputs = [75])]
    [Test(55312, "125 17", 25)]
    public long Part1(string data, int depth)
    {
        var stones = data.Split(' ').Select(i => long.Parse(i)).ToArray();
        var memo = new Dictionary<Tuple<int, long>, long>();

        return stones.Sum(s => split(depth, s));

        long split(int depthRemaining, long value)
        {
            if (depthRemaining == 0) 
            {
                return 1;
            }

            var key = Tuple.Create(depthRemaining, value);
            if (memo.ContainsKey(key)) return memo[key];

            long ret;

            if (value == 0) 
            {
                ret = split(depthRemaining - 1, 1);
            }
            else
            {
                var log = (long)Math.Log10(value);
                if (log % 2 == 0)
                {
                    ret = split(depthRemaining - 1, value * 2024);
                }
                else
                {
                    var pow = (long) Math.Pow(10, log / 2 + 1);
                    ret = split(depthRemaining - 1, value / pow)
                        + split(depthRemaining - 1, value % pow);
                }
            }

            memo[key] = ret;
            return ret;        
        }

    }
}