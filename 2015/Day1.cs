using AdventOfCode;

namespace AdventOfCode.AoC2015;

class Day1 {

    [Solution(2015, 1, 1)]
    [Test(0,"(())")]
    [Test(-3,")())())")]
    public int EndFloor(string input) 
    {
        return input.Count(c => c == '(') - input.Count(c => c == ')');
    }


    [Solution(2015, 1, 2)]
    [Test(5, "()())")]
    public int BasementStep(string input)
    {
        var floor = 0;

        for (var i = 0; i < input.Length; i++) 
        {
            floor += ((input[i] == '(') ? 1 : -1);
            if (floor == -1) return i + 1;
        }

        throw new InvalidOperationException();

    }
}

