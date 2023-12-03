namespace AdventOfCode.AoC2015;

class Day17
{

    [Solution(2015,17,1,150)]
    [Test(4, "20\n15\n10\n5\n5", 25)]
    public int CountCombinations(string[] containers, int targetVolume)
    {
        var sizes = containers.Select(c => int.Parse(c)).OrderByDescending(c => c).ToArray();

        var combinations = 0;

        findCombinations(0, 0);
        return combinations;

        void findCombinations(int index, int runningTotal)
        {
            if (runningTotal == targetVolume)
            {
                combinations++;
                return;
            }
            else if (index == sizes.Length)
            {
                return;
            }

            for (int j = 0; j <= 1; j++)
            {
                findCombinations(index + 1, runningTotal + j * sizes[index]);
            }
        }
    }

    [Solution(2015,17,2,150)]
    [Test(3, "20\n15\n10\n5\n5", 25)]
    public int CountCombinations2(string[] containers, int targetVolume)
    {
        var sizes = containers.Select(c => int.Parse(c)).OrderByDescending(c => c).ToArray();

        var combinations = 0;
        var minContainersUsed = int.MaxValue;

        findCombinations(0, 0, 0);
        return combinations;


        void findCombinations(int index, int runningTotal, int containersUsed)
        {
            if (runningTotal == targetVolume)
            {
                if (containersUsed < minContainersUsed)
                {
                    combinations = 1;
                    minContainersUsed = containersUsed;
                }
                else if (containersUsed == minContainersUsed)
                {
                    combinations++;
                }
                return;
            }
            else if (index == sizes.Length)
            {
                return;
            }

            for (int j = 0; j <= 1; j++)
            {
                findCombinations(index + 1, runningTotal + j * sizes[index], containersUsed + j);
            }
        }
    }
}