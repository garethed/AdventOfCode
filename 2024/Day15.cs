namespace AdventOfCode.AoC2024;

class Day15
{
    [Solution()]
    [Test(10092, testInput)]
    public long Part1(Input input)
    {
        var map = input.map;

      
        var pos = map.First(kv => kv.Value == robot).Key;

        foreach (var m in input.moves)
        {
            var firstGap = findGap();
            if (firstGap != null)            
            {
                if (map[pos + m] == box)
                {
                    map[firstGap.Value] = box;
                }                
                map[pos] = empty;
                pos = pos + m;
                map[pos] = robot;
            }

            Point2? findGap()
            {
                var p = pos;
                while (true)
                {
                    p += m;
                    if (map[p] == wall) return null;
                    if (map[p] == empty) return p;
                }
            }

        }

        //map.Print();

        return map.Sum (kv => kv.Value == box ? GPS(kv.Key) : 0);        

    }

    [Solution()]
    [Test(9021, testInput)]
    public long Part2(Input input)
    {
        var map = input.doubleMap;
        var pos = map.First(kv => kv.Value == robot).Key;

        foreach (var move in input.moves)
        {
            if (tryMove(pos, move, false)) 
            {
                tryMove(pos, move, true);
                pos = pos + move;
            }            

            bool tryMove(Point2 p, Point2 m, bool commit)
            {
                var canMove = (map[p + m]) switch
                {
                    empty => true,
                    wall => false,
                    '[' => (m == Point2.West || tryMove(p + m + Point2.East, m, commit)) && tryMove(p + m, m, commit), 
                    ']' => (m == Point2.East || tryMove(p + m + Point2.West, m, commit)) && tryMove(p + m, m, commit),

                };

                if (canMove && commit)
                {
                    map[p + m] = map[p];
                    map[p] = empty;
                }
                return canMove;
            }

        }

        //map.Print();

        return map.Sum (kv => kv.Value == '[' ? GPS(kv.Key) : 0);                

    }

    public int GPS(Point2 p) => p.x + 100 * p.y;

    const char box = 'O';
    const char wall = '#';
    const char empty = '.';
    const char robot = '@';

    public class Input
    {
        public CharGrid2 map;
        public CharGrid2 doubleMap;
        public List<Point2> moves;

        public Input(string raw)
        {
            var parts = Utils.SplitOnBlankLines(raw);
            map = new CharGrid2(parts[0]);
            moves = parts[1].Replace("\n", "").Select(c => Point2.FromChar(c)).ToList();

            doubleMap = new CharGrid2(
                parts[0]
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@."));
        }
    }

    private const string testInput = @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
";
}
