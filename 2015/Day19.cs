namespace AdventOfCode.AoC2015;

public class Day19
{

    [Solution(2015,19,1)]
    [Test(7, testData)]
    public int Part1(string[] input)
    {
        var substitutions = input.Take(input.Length - 2).Select(i => new Substitution(i));
        var molecule = input.Last();

        var outputs = new HashSet<string>();

        foreach (var s in substitutions)
        {
            outputs.UnionWith(s.AllSubstitutions(molecule));
        }

        return outputs.Count;
    }

    [Solution(2015,19,2)]
    [Test(1, "AA")]
    [Test(2, "XRnXArX")]
    [Test(6,"XRnXXYXXXArXX")]
    public int Part2(string[] input)
    {
        var molecule = input.Last();

        // _ => __   (+1)
        // _ => _Rn_Ar (+3)
        // _ => _Rn_Y_Ar (+5)
        // _ => _Rn_Y_Y_Ar (+7)

        var atoms = molecule.Count(c => char.IsUpper(c));
        var y = molecule.Count(c => c == 'Y');
        var rn = molecule.Count(c => c == 'R');

        return atoms - y * 2 - rn * 2 - 1;
    }

    private IEnumerable<string> splitAtRightOnly(string molecule, HashSet<string> rightOnly)
    {
        if (!rightOnly.Any()) 
        {
            yield return molecule;
            yield break;
        }

        var remaining = molecule;
        while (true)
        {
            var split = rightOnly.Select(r => remaining.IndexOf(r, 1) >= 0 ? remaining.IndexOf(r, 1) : int.MaxValue).Min();
            if (split < int.MaxValue)
            {
                yield return remaining.Substring(0, split + 2);
                remaining = remaining.Substring(split + 2);
            }
            else 
            {
                break;
            }
        }                
    }

    class Substitution
    {
        public string from;
        public string to;
        public HashSet<string> toComponents = new();

        public Substitution(string description)
        {
            var parts = description.Split(" ");
            from = parts[0];
            to = parts[2];

            for (var i = 0; i < to.Length; i++)
            {
                if (char.IsUpper(to[i]))
                {
                    if (i + 1 < to.Length && char.IsLower(to[i + 1]))
                    {
                        toComponents.Add(to.Substring(i, 2));
                    }
                    else 
                    {
                        toComponents.Add(to.Substring(i, 1));
                    }
                }
            }
        }

        public IEnumerable<string> AllSubstitutions(string input)
        {
            var i = 0;

            while(true)
            {
                var next = input.IndexOf(from,i);
                if (next >= 0)
                {
                    yield return input[..next] + to + input[(next + from.Length)..];
                }
                else 
                {
                    break;
                }
                i = next + 1;
            }            
        }

        public IEnumerable<string> ReverseSubstitutions(string input)
        {
            var i = 0;

            while(true)
            {
                var next = input.IndexOf(to,i);
                if (next >= 0)
                {
                    yield return input[..next] + from + input[(next + to.Length)..];
                }
                else 
                {
                    break;
                }
                i = next + 1;
            }            
        }

        public string ReverseApply(string input, out int rounds)
        {
            var ret = input;
            rounds = 0;

            while (true)
            {
                var next = ret.IndexOf(to);
                if (next >= 0)
                {
                    ret = ret[..next] + from + ret[(next + to.Length)..];
                    rounds++;
                }
                else 
                {
                    return ret;
                }
            }
        }


    }

    const string testData = @"e => H
e => O
H => HO
H => OH
O => HH

HOHOHO";
}