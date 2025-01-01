namespace AdventOfCode.AoC2024;

class Day21
{
    [Solution]
    [Test(126384, testInput)]
    public long Part1(string[] inputs)
    {
        var keypad = new Keypad() { map = NUMBERS, ChildKeypad = new Keypad() { map = DIRECTIONS, ChildKeypad = new Keypad() { map = DIRECTIONS, ChildKeypad = new Keypad { map = DIRECTIONS }}}};
        var mappedInputs = inputs.Select(x => keypad.Cost(x));

        return inputs.Sum(i => keypad.Cost(i) * long.Parse(i.Substring(0, 3)));          
    }

    [Solution]
    public long Part2(string[] inputs)
    {
        var keypad = new Keypad() { map = NUMBERS };

        var childKeypad = keypad;

        for (long i = 0; i < 26; i++)
        {
            var newChild = new Keypad() { map = DIRECTIONS };
            childKeypad.ChildKeypad = newChild;
            childKeypad = newChild;
        }
        
        return inputs.Sum(i => keypad.Cost(i) * long.Parse(i.Substring(0, 3)));          
    }

    class Keypad
    {
        public string map;
        public Keypad ChildKeypad;

        int X (int i) => (i % 3);
        int Y (int i) => (i / 3);

        Dictionary<string,long> costs = [];

        public long Cost(string targetOutput)
        {
            if (ChildKeypad == null) 
            {
                return targetOutput.Length;
            }

            if (costs.TryGetValue(targetOutput, out var cached))
            {
                return cached;
            }

            var p = map.IndexOf(A);
            var cost = 0L;
            foreach (var c in targetOutput)
            {
                var t = map.IndexOf(c);
                var dx = X(t) - X(p);
                var dy = Y(t) - Y(p);
                var canDoXFirst = map[p + dx] != '_';
                var canDoYFirst = map[p + dy * 3] != '_';

                var xmoves = "";
                while (dx != 0)
                {
                    xmoves += (dx > 0) ? '>' : '<';
                    dx -= Math.Sign(dx);
                }

                var ymoves = "";

                while (dy != 0)
                {
                    ymoves += (dy > 0) ? 'v' : '^';
                    dy -= Math.Sign(dy);
                }

                var minCost = long.MaxValue;

                if (canDoXFirst)
                {
                    minCost = ChildKeypad.Cost(xmoves + ymoves + 'A');                    
                }                
                if (canDoYFirst)
                {
                    minCost = Math.Min(minCost, ChildKeypad.Cost(ymoves + xmoves + 'A'));
                }

                cost += minCost;

                p = t;
            }    

            costs[targetOutput] = cost;
            return cost;           
        }        
    }
    
    const string NUMBERS = "789456123_0A";
    const string DIRECTIONS = "_^A<v>";

    const char A = 'A';

    private const string testInput = @"029A
980A
179A
456A
379A";
}