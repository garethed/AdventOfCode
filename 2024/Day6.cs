namespace AdventOfCode.AoC2024;

class Day6
{
    [Solution]
    [Test(41, testData)]
    public int Part1(CharGrid2 input)
    {        
        return FindVisited(input).Count();
    }

    public HashSet<Point2> FindVisited(CharGrid2 input)
    {
        var p = input.Points.Where(x => input[x] == '^').First();
        var d = Point2.North;

        var visited = new HashSet<Point2>();

        while (input.Contains(p + d))
        {
            visited.Add(p);

            while (input[p + d] == '#') 
            {
                d = d.Clockwise();
            }
            p = p + d;
        }        

        visited.Add(p);

        return visited;
    }

    [Solution]
    [Test(6, testData)]
    public int Part2(CharGrid2 input)
    {        
        var visited = FindVisited2(input);
        return visited.Count(pd => !LeavesGrid(input, pd.Key, pd.Value));
    }    

    public Dictionary<Point2, Point2> FindVisited2(CharGrid2 input)
    {
        var p = input.Points.Where(x => input[x] == '^').First();
        var d = Point2.North;

        var visited = new Dictionary<Point2, Point2>();

        while (input.Contains(p + d))
        {
            if (!visited.ContainsKey(p))
            {
                visited[p] = d;
            }

            while (input[p + d] == '#') 
            {
                d = d.Clockwise();
            }
            p = p + d;
        }    

        visited[p] = d;

        return visited;
    }    

    public bool LeavesGrid(CharGrid2 input, Point2 blockagePoint, Point2 blockageDirection)
    {
        var p = blockagePoint - blockageDirection;
        var d = blockageDirection.Clockwise();

        var visited = new HashSet<Tuple<Point2,Point2>>();

        while (input.Contains(p + d))
        {
            var t = Tuple.Create(p, d);
            if (visited.Contains(t)) return false;
            visited.Add(t);

            while (input[p + d] == '#' || p + d == blockagePoint) 
            {
                d = d.Clockwise();
            }
            p = p + d;
        }        

        return true;
    }

    private const string testData = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
";
}