using System.Data;
using System.Dynamic;

namespace AdventOfCode.AoC2023;

public class Day9
{
    [Solution(2023,9,1)]
    [Test(114, testData)]
    public long Part1(string[] input)
    {
        var data = input.Select(i => i.ToLongArray()).ToArray();
        var sequences = data.Select(d => new SequenceData(d));

        return sequences.Sum(s => s.GetValue(s.initialCount));
    }

    [Solution(2023,9,2)]
    [Test(2, testData)]
    public long Part2(string[] input)
    {
        var data = input.Select(i => i.ToLongArray()).ToArray();
        var sequences = data.Select(d => new SequenceData(d));

        return sequences.Sum(s => s.GetPrevious());
    }

    private static Dictionary<long,long> coefficientCache = new Dictionary<long, long>();

    public static long GetCoefficient(int row, int column)
    {
        if (column == 0) return 1;
        if (column > row) return 0;

        var key = (long)row * 1000000 + (long)column;

        if (!coefficientCache.ContainsKey(key))
        {
            coefficientCache[key] = GetCoefficient(row - 1, column - 1) + GetCoefficient(row - 1, column);
        }

        return coefficientCache[key];
    }

    class SequenceData
    {
        public List<long> initialValues = new List<long>();
        public int initialCount;

        public SequenceData(long[] initialSequence)
        {
            initialCount = initialSequence.Length;

            while (initialSequence.Any(i => i != 0L))
            {
                initialValues.Add(initialSequence[0]);
                initialSequence = initialSequence.Zip(initialSequence.Skip(1), (first, second) => second - first).ToArray();
            }       
        }

        public long GetValue(int index)
        {
            var ret = 0L;

            for (int c = 0; c < initialValues.Count; c++)
            {
                ret += GetCoefficient(index, c) * initialValues[c];
            }

            return ret;
        }

        public long GetPrevious()
        {
            var next = 0L;

            for (int i = initialValues.Count - 1; i >= 0; i--)
            {
                next = initialValues[i] - next;
            }

            return next;            
        }

    }

    private const string testData = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
";
}