namespace AdventOfCode.AoC2015;

public class Day24
{
    [Solution(2015,24,1, 3)]
    [Solution(2015,24,2, 4)]
    [Test(99, new long[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11}, 3)]
    public long Part1(long[] weights, int groups)
    {
        var target = weights.Sum() / groups;        
        var minGroupSize = long.MaxValue;

        weights = weights.OrderByDescending(w => w).ToArray();

        var minSizeGroups = new List<long[]>();

        choosePackages(new HashSet<long>(), target, long.MaxValue);

        return minSizeGroups.Min(g => entanglement(g) );

        
        long entanglement(long[] ints)
        {
            return ints.Aggregate(1L, (total, i) => total * i);
        }
        
        void choosePackages(HashSet<long> chosen, long remaining, long smallestChosen)
        {
            if (remaining == 0)
            {
                minGroupSize = chosen.Count;
                minSizeGroups.Add(chosen.ToArray());
            }
            else if (chosen.Count == minGroupSize)
            {
                return;
            }

            foreach (var weight in weights)
            {
                if (weight <= remaining  && weight < smallestChosen && !chosen.Contains(weight))
                {
                    chosen.Add(weight);
                    choosePackages(chosen, remaining - weight, weight);
                    chosen.Remove(weight);
                }
            }
        }



    }
}
