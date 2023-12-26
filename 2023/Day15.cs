using System.Collections;

namespace AdventOfCode.AoC2023;

public class Day15
{
    [Solution(2023,15,1)]
    [Test(1320,"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7")]
    public long Part1(string input)
    {
        return input.Split(',').Select(s => HASH(s)).Sum();
    }

    private int HASH(string input)
    {
        var hash = 0;
        foreach (var c in input)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    [Solution(2023,15,2)]
    [Test(145,"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7")]
    public long Part2(string input)
    {
        var boxes = Enumerable.Range(0,256).Select(i => new List<Tuple<string,int>>()).ToArray();

        foreach (var command in input.Split(','))
        {
            if (command.EndsWith("-"))
            {
                var code = command.Substring(0, command.Length - 1);
                var box = HASH(code);
                var existing = boxes[box].FindIndex(t => t.Item1 == code);
                if (existing >= 0)
                {
                    boxes[box].RemoveAt(existing);
                }            
            }
            else 
            {
                var parts = command.Split("=");
                var code = parts[0];
                var lens = int.Parse(parts[1]);
                var box = HASH(code);
                var existing = boxes[box].FindIndex(t => t.Item1 == code);
                if (existing >= 0)
                {
                    boxes[box].RemoveAt(existing);
                    boxes[box].Insert(existing, Tuple.Create(code,lens));
                }            
                else 
                {
                    boxes[box].Add(Tuple.Create(code,lens));
                }
            }
        }

        var sum = 0;

        for (var b = 0; b < 256; b++)
        {
            var box = boxes[b];
            for (var s = 0; s < box.Count; s++)
            {
                sum += (b + 1) * (s + 1) * box[s].Item2;
            }
        }

        return sum;

    }
}