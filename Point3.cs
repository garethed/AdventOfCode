namespace AdventOfCode;

public record struct Point3(long x, long y, long z)
{
    public static Point3 FromString(string input)
    {
        var points = input.Split(',', StringSplitOptions.TrimEntries).Select(s => long.Parse(s)).ToArray();
        return new Point3(points[0], points[1], points[2]);
    }

    public long Abs() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

    public static Point3 operator + (Point3 a, Point3 b) => new Point3(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Point3 Down = new Point3 (0, 0, -1);
    public static Point3 Up = new Point3 (0, 0, 1);

}