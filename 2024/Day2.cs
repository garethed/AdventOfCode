namespace AdventOfCode.AoC2024;

class Day2 
{
    [Solution(2024,2,1)]
    [Test(2, testData)]
    public int Part1(Report[] input)
    {
        return input.Count(r => r.Safe);        
    }

    [Solution(2024,2,2)]
    [Test(4, testData)]
    public int Part2(Report[] input)
    {
        return input.Count(r => r.SafeAfterDampening);
    }

    public class Report
    {
        public List<int> levels;
        public Report(string input)
        {
            levels = input.Split(" ").Select(x => int.Parse(x)).ToList();
        }

        public bool Safe 
        {
            get 
            {
                var diffs = levels.Pairwise((fst,snd) => fst - snd);
                return diffs.All(x => x >= 1 && x <= 3) || diffs.All(x => x <= -1 && x >= -3);
            }
        }

        public bool SafeAfterDampening
        {
            get 
            {
                if (Safe) return true;

                for (int i = 0; i < levels.Count; i++)
                {
                    var removed = levels[i];
                    levels.RemoveAt(i);

                    var diffs = levels.Pairwise((fst,snd) => fst - snd);
                    if (diffs.All(x => x >= 1 && x <= 3) || diffs.All(x => x <= -1 && x >= -3))
                    {
                        return true;
                    }

                    levels.Insert(i, removed);
                }

                return false;
            }
        }




        override public string ToString() { return Utils.describe(levels); }
    }

    private const string testData = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
";
    
}
