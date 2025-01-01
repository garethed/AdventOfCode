namespace AdventOfCode.AoC2024;

class Day17
{
    [Solution()]
    [Test(new long[] {4,6,3,5,6,3,5,2,1,0}, testInput)]
    public long[] Part1(ProgramState programState)
    {
        return programState.Run();
    }

    [Solution()]
    [Test(117440, testInput2)]
    public long Part2(ProgramState programState)
    {
        var target = programState.Instructions;
        var elements = new List<long>();

        Console.WriteLine("target: " + Utils.describe(programState.Instructions));

        try
        {
            for (long i = 0; i < 1 << 10; i++)
            {
                var output = programState.Run(i);
                if (output.Last() == target.Last())
                {
                    extend(i, 1);
                }
            }
        }
        catch (CompletedException e)
        {
            var check = programState.Run(e.Result);

            return e.Result;
        }

        return -1;

        void extend(long prev, int matchesSoFar)
        {
            var start = prev << 3;
            for (int i = 0; i < 8; i++)
            {
                var output = programState.Run(start + i);
                if (output.Length > matchesSoFar + 1 && output[output.Length - matchesSoFar] == target[target.Length - matchesSoFar])
                {
                    if (output.Length == target.Length)
                    {
                        if (Enumerable.SequenceEqual(output, target))
                        {
                            throw new CompletedException(start + i);
                        }
                    }
                    else
                    {
                        extend(start + i, matchesSoFar + 1);
                    }
                }
            }
        }
    }

    class CompletedException(long result) : Exception { 
        public long Result => result;
    }

    [RegexDeserializable(@"Register A: (\d+)\nRegister B: (\d+)\nRegister C: (\d+)\n\nProgram: ([0-9\,]+)")]
    public class ProgramState(long a, long b, long c, string instructions)
    {
        public long A = a;
        public long B = b;
        public long C = c;
        public long[] Instructions = instructions.Split(',').Select(c => long.Parse(c)).ToArray();

        long pc = 0;

        public List<long> outputs = new List<long>();

        public void Reset() 
        {
            A = a;
            B = b;
            C = c;
            pc = 0;
            outputs = new List<long>();
        }

        public long[] Run(long overrideA = long.MinValue)
        {
            Reset();
            if (overrideA >= 0)
            {
                A = overrideA;
            }

            Action<long>[] ops = [
                (o) => A = A / (long)Math.Pow(2, combo(o)),
                (o) => B = B ^ o,
                (o) => B = combo(o) % 8,
                (o) => { if (A != 0) { pc = o - 2; }},
                (o) => B = B ^ C,
                (o) => outputs.Add(combo(o) % 8),
                (o) => B = A / (long)Math.Pow(2, combo(o)),
                (o) => C = A / (long)Math.Pow(2, combo(o)),
                _ => throw new Exception()
            ];            

            while (pc < Instructions.Length - 1)
            {
                ops[Instructions[pc]](Instructions[pc + 1]);
                pc += 2;

            }       

            return outputs.ToArray();     
        }

        private long combo(long p) 
        {
            return p switch {
                <= 3 => p,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new Exception()
            };
        }

    }

    private const string testInput = @"Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0";

    private const string testInput2 = @"Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0";
}