namespace AdventOfCode.AoC2024;

class Day22
{
    [Solution]
    [Test(37327623, "1\n10\n100\n2024")]
    public long Part1(long[] inputs)
    {
        return inputs.Sum(i => generation(i, 2000));
    }

    [Solution]
    [Test(23, "1\n2\n3\n2024")]
    public long Part2(long[] inputs)
    {
        var changeStrings = new Dictionary<string, int>();
        
        foreach (var input in inputs) { buildSequences(input); }

        var max = changeStrings.MaxBy(kv => kv.Value);
        return max.Value;

        void buildSequences(long input)
        {
            var prev = input;
            var rolling = "";

            var changeStringsForThisInput = new HashSet<string>();

            for (int i = 0; i < 2000; i++)
            {
                input = next(input);
                rolling += chars[(int)(input % 10) - (int)(prev % 10) + 9];

                if (rolling.Length > 4)
                {
                    rolling = rolling.Substring(1);
                }
                if (rolling.Length == 4)
                {
                    if (!changeStringsForThisInput.Contains(rolling))
                    {
                        if (changeStrings.ContainsKey(rolling))
                        {
                            changeStrings[rolling] += (int)(input % 10);
                        }
                        else 
                        {
                            changeStrings.Add(rolling, (int)(input % 10));
                        }                    
                        changeStringsForThisInput.Add(rolling);
                    }

                }                

                prev = input;
            }
        }        
        
    }    

    string chars = "9876543210ABCDEFGHI";

    long generation(long input, int generations)
    {
        for (int i = 0; i < 2000; i++)
        {
            input = next(input);
        }

        return input;
    }

    long next(long input)
    {
        var i = input;
        i = (i << 6) ^ i;    
        i %= 16777216;
        i = (i >> 5) ^ i;
        i %= 16777216;
        i = (i << 11) ^ i;    
        i %= 16777216;
        return i;
    }
        
        
}
