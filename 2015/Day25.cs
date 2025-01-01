namespace AdventOfCode.AoC2015;

public class Day25
{

    [Solution(2015,25,1,3010,3019)]
    [Test(27995004, "", 6, 6)]
    public long Part1(string ignored, int targetRow, int targetCol)
    {        
        int row = 2, col = 1;
        long current = 20151125;
        while (row != targetRow || col != targetCol )
        {
            current = (current * 252533L) % 33554393;
            if (row > 1)
            {
                row -= 1;
                col += 1;
            }
            else
            {
                row = col + 1;
                col = 1;
            }
        }

        return (current * 252533L) % 33554393;
    }
}