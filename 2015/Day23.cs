namespace AdventOfCode.AoC2015;

public class Day23
{
    [Solution(2015,23,1, 0)]
    [Solution(2015,23,2, 1)]
    [Test(2, testData, 0)]
    public long Part1(string[] input, int startingValue)
    {
        int[] registers = new int[2];
        int pc = 0;
        registers[0] = startingValue;

        var functions = new Dictionary<string,Action<int,int>> {
            ["inc"] = (r,i) => { registers[r]++; pc++; },
            ["hlf"] = (r,i) => { registers[r] /= 2; pc++; },
            ["tpl"] = (r,i) => { registers[r] *= 3; pc++; },
            ["jmp"] = (r,i) => pc += i,
            ["jio"] = (r,i) => { if (registers[r] == 1) pc += i; else pc++; },
            ["jie"] = (r,i) => { if (registers[r] % 2 == 0) pc += i; else pc++; },
        };

        var program = input.Select(s => parseStatement(s)).ToArray();

        while (pc >= 0 && pc < program.Length)
        {
            var s = program[pc];
            s.invoke();
        }

        return registers[1];






        Statement parseStatement(string s)
        {
            
            var parts = s.Replace(",", "") .Split(' ');
            var action = functions[parts[0]];
            var p1 = parts[1].Length == 1 ? "ab".IndexOf(parts[1][0]) : 0;
            var p2 = parts.Length > 2 ? int.Parse(parts[2]) : 0;
            if (parts[1].Length > 1) p2 = int.Parse(parts[1]);
            
            return new Statement(s, action, p1, p2);
        }
    }
    

    record Statement(string code, Action<int,int> action, int p1, int p2) {
        public void invoke() { action(p1, p2); }
    }

    private const string testData = @"inc b
jio b, +2
tpl b
inc b
";

}