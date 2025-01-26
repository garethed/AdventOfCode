namespace AdventOfCode.AoC2017;

public class Day22
{
    [Solution(additionalInputs = [10000])]
    [Test(41, testData, 70)]
    [Test(5587, testData, 10000)]
    public long Part1(CharGrid2 input, int iterations)
    {
        var infected = input.Where(kv => kv.Value == '#').Select(kv => kv.Key).ToHashSet();
        var p = new Point2(input.Width / 2, input.Height / 2);
        var d = Point2.North;
        var b = 0;

        for (int i = 0; i < iterations; i++)
        {
            if (infected.Contains(p))
            {
                d = d.Clockwise();
                infected.Remove(p);
            }
            else 
            {
                d = d.Anticlockwise();
                infected.Add(p);
                b++;
            }

            p += d;
        }

        return b;
    }

    [Solution(additionalInputs = [10000000])]
    [Test(26, testData, 100)]
    [Test(2511944, testData, 10000000)]
    public long Part2(CharGrid2 input, int iterations)
    {
        var infected = input.Where(kv => kv.Value == '#').Select(kv => kv.Key).ToDictionary(p => p, p => 2);
        var p = new Point2(input.Width / 2, input.Height / 2);
        var d = Point2.North;
        var b = 0;

        for (int i = 0; i < iterations; i++)
        {
            if (infected.TryGetValue(p, out int state))
            {
                switch (state)
                {
                    case 1:
                        infected[p] = 2;
                        b++;
                        break;
                    case 2:
                        d = d.Clockwise();
                        infected[p] = 3;
                        break;
                    case 3:
                        d *= -1;
                        infected.Remove(p);
                        break;
                }                
            }
            else 
            {
                d = d.Anticlockwise();
                infected[p] = 1;
            }

            p += d;
        }

        return b;
    }


    const string testData = @"..#
#..
...";
}
