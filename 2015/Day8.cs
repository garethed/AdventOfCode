using System.Text;

namespace AdventOfCode.AoC2015;

public class Day8
{
    [Solution(2015,8,1)]
    public int Part1(string[] input)
    {
        return input.Sum(s => s.Length - unescape(s).Length);
    }

    [Solution(2015,8,2)]
    [Test(5, @"""\x27""")]
    public int Part2(string[] input)
    {
        return input.Sum(s => 2 + s.Count(c => c == '\\' || c == '"'));
    }

    [Test("'", @"""\x27""")]
    public static string unescape(string input)
    {
        var sb = new StringBuilder();
        var i = 1;

        while (i < input.Length - 1)
        {
            if (input[i] == '\\')
            {
                if (input[i+1] == 'x')
                {
                    var code = int.Parse(input.Substring(i + 2, 2), System.Globalization.NumberStyles.HexNumber);
                    sb.Append((char)code);
                    i += 4;
                }
                else 
                {
                    sb.Append(input[i + 1]);
                    i += 2;
                }

            }
            else {
                sb.Append(input[i]);
                i++;
            }            
        }      

        return sb.ToString();          
    }

}