namespace AdventOfCode.AoC2024;

class Day25
{
    [Solution]
    [Test(3, testInput)]
    public long Part1(string input)
    {
        var diagrams = Utils.SplitOnBlankLines(input);
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        foreach (var diagram in diagrams)
        {
            var lines = Utils.splitLines(diagram);

            bool islock = lines[0][0] == '#';
            int[] heights = new int[lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        heights[x]++;
                    }
                }
            }

            if (islock)
            {
                locks.Add(heights);
            }
            else
            {
                keys.Add(heights);
            }
        }

        return locks.Sum(l => keys.Count(k => k[0] + l[0] < 8 && k[1] + l[1] < 8 && k[2] + l[2] < 8 && k[3] + l[3] < 8 && k[4] + l[4] < 8));
    }

    const string testInput = @"#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####
";
}
