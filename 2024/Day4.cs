namespace AdventOfCode.AoC2024;

class Day4
{
    [Solution(2024,4,1)]
    [Test(18, testData)]
    public int Part1(CharGrid2 input)
    {
        string word = "XMAS";
        return input.Points
            .Where(p => word[0] == input[p])
            .Sum(p => Point2.DirectionsWithDiagonals.Count(d => check(p + d, d, 1)));

        bool check(Point2 p, Point2 d, int i)
        {
            if (i == word.Length) 
            { 
                //Utils.WriteLine($"{p - 4 * d} {d}", ConsoleColor.Gray); 
                return true; 
            }
            if (!input.Contains(p) || input[p] != word[i]) return false;
            return check(p + d, d, i + 1);
        }
    }

    [Solution(2024,4,2)]
    [Test(9, testData)]
    public int Part2(CharGrid2 input)
    {
        
        return input.Points
            .Where(p => 'A' == input[p])
            .Sum(p => Point2.Diagonals.Count(d => check(p, d)));

        bool check(Point2 p, Point2 d)
        {
            return input.GetOrDefault(p + d, '@') == 'M' 
            && input.GetOrDefault(p + d.Clockwise(), '@') == 'M'
            && input.GetOrDefault(p + d.Clockwise().Clockwise(), '@') == 'S'
            && input.GetOrDefault(p + d.Anticlockwise(), '@') == 'S';                        
        }
    }



    private const string testData = 
@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";
}