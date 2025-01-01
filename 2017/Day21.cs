namespace AdventOfCode.AoC2017;

public class Day21
{
    [Solution(2017,21,1,5)]
    [Test(12, testData, 3)]
    public long Part1(string[] input)
    {
        var rules = new Dictionary<string,string>();

        foreach (var line in input)
        {
            var parts = line.Replace("/", "").Split(" => ");
            var from = parts[0];
            var to = parts[1];
            foreach (var transformedSource in allCombinations(from))
            {
                rules[transformedSource] = to;
            }        
        }

        return 0L;


    }

    private IEnumerable<string> splitGrid(string grid)
    {
        var subgridsize = grid.Length % 2 == 0 ? 2 : 3;
        var dim = grid.Length / subgridsize;
        for (int y = 0; y < dim; y++)
        {
            for (int x = 0; x < dim; x++)
            {
                yield return "qq"; //grid.Substring(y * subgridsize * dim);

            }
        }
    }


    private string rotateLeft(string rule)
    {
        if (rule.Length == 4)
        {
            return new string(new[] { 2,4,1,3 }.Select(i => rule[i - 1]).ToArray());
        }
        else
        {
            return new string(new[] { 3,6,9,2,5,8,1,4,7 }.Select(i => rule[i - 1]).ToArray());
        }
    }

    private string reflectX(string rule)
    {
        if (rule.Length == 4)
        {
            return new string(new[] { 2,1,4,3 }.Select(i => rule[i - 1]).ToArray());
        }
        else
        {
            return new string(new[] { 3,2,1,6,5,4,9,8,7 }.Select(i => rule[i - 1]).ToArray());
        }
    }

    private IEnumerable<string> allCombinations(string initial)
    {
        yield return initial;
        var f = reflectX(initial);
        yield return f;
        var r = rotateLeft(initial);
        yield return r;
        var fr = rotateLeft(r);
        yield return reflectX(r);
        yield return reflectX(fr);
        yield return rotateLeft(r);
        yield return rotateLeft(fr);
    }
    

    private const string testData = @"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#";
}