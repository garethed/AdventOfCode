namespace AdventOfCode.AoC2017;

public class Day25
{
    [Solution]
    [Test(3, testInput)]
    public long Part1(TuringMachine machine)
    {

        var state = machine.States['A'];
        var i = 0;
        var pos = machine.Iterations;
        bool[] tape = new bool[machine.Iterations * 2];

        while (i < machine.Iterations)
        {            
            var value = tape[pos];

            if (value)
            {
                tape[pos] = state.writeIfOne > 0;
                pos += state.moveIfOne;
                state = machine.States[state.nextIfOne];
            }
            else
            {
                tape[pos] = state.writeIfZero > 0;
                pos += state.moveIfZero;
                state = machine.States[state.nextIfZero];
            }

            i++;
        }

        return tape.Count(i => i);
    }

    public class TuringMachine
    {
        public Dictionary<char, State> States = [];
        public int Iterations;

        public TuringMachine(string[] input)
        {
            input = input.Select(l => l.TrimEnd('.', ':')).ToArray();

            var i = 0;

            foreach (var line in input)
            {
                if (line.EndsWith(" steps"))
                {
                    Iterations = int.Parse(line.Split(' ')[5]);
                }
                else if (line.StartsWith("In state "))
                {
                    var state = new State(
                            line.Last(),
                            int.Parse(input[i + 2].Split(' ').Last()),
                            input[i + 3].Split(' ').Last() == "right" ? 1 : -1,
                            input[i + 4].Last(),
                            int.Parse(input[i + 6].Split(' ').Last()),
                            input[i + 7].Split(' ').Last() == "right" ? 1 : -1,
                            input[i + 8].Last());

                    States.Add(state.Id, state);
               }

               i++;
            }
        }

    }

    public record State(char Id, int writeIfZero, int moveIfZero, char nextIfZero, int writeIfOne, int moveIfOne, char nextIfOne);


    const string testInput = @"Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.
";
}
