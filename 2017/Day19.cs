namespace AdventOfCode.AoC2017;

public class Day19
{
    [Solution]
    [Test("ABCDEF", testData)]
    public string Part1(string inputString)
    {
        var input = new CharGrid2(Utils.SanitizeInput(inputString).Split('\n'));
        var pos = input.Points.Where(p => p.y == 0 && input[p] == '|').First();
        var direction = Point2.South;
        var output = "";
        steps = 0;

        while (input.Contains(pos))
        {
            var cell = input[pos];
            if (char.IsLetter(cell)) 
            {
                output += cell;
            }
            else if (cell == '+')
            {
                direction = direction.Clockwise();
                if (!input.Contains(pos + direction) || input[pos + direction] == ' ')
                {
                    direction = -1 * direction;
                }
            }
            else if (cell == ' ')
            {
                break;
            }
            

            pos += direction;
            steps++;
        }

        return output;        
    }

    [Solution]
    [Test(38, testData)]
    public int Part2(string input)
    {
        Part1(input);
        return steps;
    }

    int steps;

    private const string testData = @"     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 

";
}