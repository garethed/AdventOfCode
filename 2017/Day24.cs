namespace AdventOfCode.AoC2017;

public class Day24
{
    [Solution]
    [Test(31, testInput)]
    public long Part1(string[] input)
    {
        var connections = buildConnections(input);

        var used = new HashSet<string>();
        var maxStrength = 0;

        extend(0, 0);

        return maxStrength;

        void extend(int current, int strength)
        {
            if (strength > maxStrength)
            {
                maxStrength = strength;
            }

            foreach (var next in connections[current])
            {
                var connectionId = Id(current, next);
                if (!used.Contains(connectionId))
                {
                    used.Add(connectionId);
                    extend(next, strength + current + next);
                    used.Remove(connectionId);
                }
            }

        }
    }

    [Solution]
    [Test(19, testInput)]
    public long Part2(string[] input)
    {
        var connections = buildConnections(input);

        var used = new HashSet<string>();
        var maxStrength = 0;
        var maxLength = 0;

        extend(0, 0);

        return maxStrength;

        void extend(int current, int strength)
        {
            var length = used.Count;
            if (length > maxLength || (length == maxLength && strength > maxStrength))
            {
                maxStrength = strength;
                maxLength = length;
            }

            foreach (var next in connections[current])
            {
                var connectionId = Id(current, next);
                if (!used.Contains(connectionId))
                {
                    used.Add(connectionId);
                    extend(next, strength + current + next);
                    used.Remove(connectionId);
                }
            }
        }
    }


    private string Id(int a, int b)
    {
        return a < b ? $"{a}/{b}" : $"{b}/{a}";
    }

    private SetValuedDictionary<int,int> buildConnections(string[] input)
    {
        var ret = new SetValuedDictionary<int,int>();

        foreach (var s in input)
        {
            var parts = s.Split('/').Select(p => int.Parse(p)).ToArray();
            ret.Add(parts[0], parts[1]);
            ret.Add(parts[1], parts[0]);                    
        }
        return ret;
    }

    const string testInput = @"0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10";
}
