using System.Runtime.Intrinsics.X86;

namespace AdventOfCode.AoC2024;

class Day13
{
    [Solution()]
    [Test(480, testInput)]
    public long Part1(List<Instruction> instructions)
    {
        return instructions.Sum(i => i.stepsToSolve(0));
    }

    [Solution()]
    public long Part2(List<Instruction> instructions)
    {
        return instructions.Sum(i => i.stepsToSolve(10000000000000));
    }    


    [RegexDeserializable(@"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)")]
    public record Instruction(long xa, long ya, long xb, long yb, long xt, long yt)
    {
        public long stepsToSolve(long delta) 
        {
            var num = xa * (yt + delta) - ya * (xt + delta);
            var denom = xa * yb - ya * xb;

            if (num % denom == 0)
            {
                var b = num / denom;
                var num2 = (xt + delta) - b * xb;
                if (num2 % xa == 0)
                {
                    var a = num2 / xa;
                    return a * 3 + b;
                }
            }

            return 0;

        }
    }    

    private const string testInput = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279
";
}