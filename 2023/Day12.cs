namespace AdventOfCode.AoC2023;

public class Day12
{
    [Solution(2023,12,1)]
    [Test(21, testData)]
    public long Part1(string[] input)
    {
        return input.Sum(i => countPossibilities(i, 1));
    }

    [Solution(2023,12,2)]
    [Test(525152, testData)]
    public long Part2(string[] input)
    {
        return input.Sum(i => countPossibilities(i, 5));
    }    

    private long countPossibilities(string input, int copies)
    {
        var parts = input.Split(" ");
        var springs = string.Join('?', Enumerable.Repeat(parts[0],copies)).ToCharArray();
        var groups = string.Join(',', Enumerable.Repeat(parts[1], copies)).ToIntArray(",");
        var springGroups = string.Join('?', Enumerable.Repeat(parts[0],copies)).Split('.', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToCharArray()).ToArray();

        return tryExpand(0, 0, 0, new Dictionary<long, long>());      

        long tryExpand(int nextSpring, int nextGroup, int currentGroupSize, Dictionary<long,long> memoised)
        {
            var key = (long)nextSpring << 32 | (long)nextGroup << 16 | (long)currentGroupSize;
            if (memoised.ContainsKey(key)) return memoised[key];

            long completed = 0L;

            if (nextSpring == springs.Length) 
            {
                if (currentGroupSize == 0 && nextGroup == groups.Length)
                {
                    memoised[key] = 1;
                    return 1;
                }
                else if (nextGroup == groups.Length - 1 && currentGroupSize == groups[nextGroup])
                {
                    memoised[key] = 1;
                    return 1;
                }

                memoised[key] = 0;
                return 0;
            }

            var spring = springs[nextSpring];

            if (spring != '#')
            {
                // assume .
                if (currentGroupSize == 0)
                {
                    completed += tryExpand(nextSpring + 1, nextGroup, 0, memoised);
                }
                else if (currentGroupSize == groups[nextGroup])
                {
                    completed += tryExpand(nextSpring + 1, nextGroup + 1, 0, memoised);
                }                                
            }
            if (spring != '.' && nextGroup < groups.Length)
            {
                // assume #
                if (currentGroupSize == 0)
                {
                    completed += tryExpand(nextSpring + 1, nextGroup, 1, memoised);
                }
                else if (currentGroupSize < groups[nextGroup])
                {
                    completed += tryExpand(nextSpring + 1, nextGroup, currentGroupSize + 1, memoised);
                }    
            }

            memoised[key] = completed;
            return completed;
        }        
    }

    private const string testData = @"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
";
}