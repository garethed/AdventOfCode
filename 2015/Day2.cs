namespace AdventOfCode.AoC2015;

class Day2 
{
    [Solution(2015,2,1)]
    public int TotalArea(string[] input)
    {
        return input.Sum(i => SinglePackage(i));
    }

    [Solution(2015,2,2)]
    public int TotalRibbon(string[] input)
    {
        return input.Sum(i => SingleRibbon(i));
    }


    public static int[] dimensionsFromString(string dimensionString)
    {
        return dimensionString.Split('x').Select(i => int.Parse(i)).OrderBy(i => i).ToArray();
    }


    [Test(58, "2x3x4")]
    public static int SinglePackage(string dimensionString)
    {
        var dimensions = dimensionsFromString(dimensionString);

        return dimensions[0] * dimensions [1] * 3
        + dimensions[1] * dimensions [2] * 2
        + dimensions[2] * dimensions [0] * 2;
    }

    [Test(34, "2x3x4")]
    public static int SingleRibbon(string dimensionString)
    {
        var dimensions = dimensionsFromString(dimensionString);
        return (dimensions[0] + dimensions[1]) * 2 + dimensions[0] * dimensions[1] * dimensions[2];
    }
}