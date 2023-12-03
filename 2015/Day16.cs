using System.Text.RegularExpressions;

namespace AdventOfCode.AoC2015;

class Day16
{

    [Solution(2015,16,1)]
    public int whichSue(string[] input)
    {
        var sues = input.Select(i => parseInput(i)).ToList();

        foreach (var kv in facts)
        {
            sues = sues.Where(s => !s.ContainsKey(kv.Key) || kv.Value(s[kv.Key])).ToList();
        }

        return sues[0]["id"];
    }

    [Solution(2015,16,2)]
    public int whichSue2(string[] input)
    {
        facts["cats"] = x => x > 7;
        facts["trees"] = x => x > 3;
        facts["pomeranians"] = x => x < 3;
        facts["goldfish"] = x => x < 5;
        return whichSue(input);
    }    

    private Dictionary<string,Func<int,bool>> facts = new Dictionary<string,Func<int,bool>>() {
            ["children"] = x => x == 3,
            ["cats"] = x => x == 7,
            ["samoyeds"] = x => x == 2,
            ["pomeranians"] = x => x == 3,
            ["akitas"] = x => x == 0,
            ["vizslas"] = x => x == 0,
            ["goldfish"] = x => x == 5,
            ["trees"] = x => x == 3,
            ["cars"] = x => x == 2,
            ["perfumes"] = x => x == 1,
        };   

    private Dictionary<string,int> parseInput(string input)
    {
        // Sue 444= trees= 2, goldfish= 7, cars= 8

        var ret = new Dictionary<string, int>();
        var parts = input.Replace(":", "").Replace(",", "").Split(' ');
        ret.Add(parts[2], int.Parse(parts[3]));
        ret.Add(parts[4], int.Parse(parts[5]));
        ret.Add(parts[6], int.Parse(parts[7]));
        ret.Add("id", int.Parse(parts[1]));
        

        return ret;
    }

}