namespace AdventOfCode.AoC2015;

public class Day3
{
    [Solution(2015,3,1)]
    [Test(4, "^>v<")]
    [Test(2, "^v^v^v^v^v")]
    public int CountVisited(string input)
    {
        return GetVisited(input).Count;
    }

    private HashSet<Point2> GetVisited(string input, int start = 0, int step = 1)
    {
        var visited = new HashSet<Point2>();
        var current = new Point2(0,0);
        visited.Add(current);

        for (var i = start; i < input.Length; i += step)
        {
            var move = input[i];
            current = move switch 
            {
                '<' => current.Dx(-1),
                '>' => current.Dx(1),
                '^' => current.Dy(-1),
                'v' => current.Dy(1),
                _ => throw new InvalidDataException()
            };

            visited.Add(current);

        }        

        return visited;
    }

    [Solution(2015,3, 2)]
    [Test(3, "^>v<")]
    [Test(11, "^v^v^v^v^v")]
    public int TwoSantas(string input)
    {
        var visited = GetVisited(input, 0, 2);
        visited.UnionWith(GetVisited(input,1,2));
        return visited.Count;
    }
}