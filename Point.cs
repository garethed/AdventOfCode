using System.Drawing;

namespace AdventOfCode;

public record struct Point2(int x, int y)
{
    public Point2 Dx(int dx) => new Point2(x + dx, y);
    public Point2 Dy(int dy) => new Point2(x, y + dy);

    public Point2 Clockwise() => new Point2(-y, x);
    public Point2 Anticlockwise() => new Point2(y, -x);
    

    public static IEnumerable<Point2> enumerateGrid(int width, int height) 
    {
        foreach (var y in Enumerable.Range(0, height)) 
        {
            foreach (var x in Enumerable.Range(0, width)) 
            {
                yield return new Point2(x,y);
            }
        }
    }    

    public IEnumerable<Point2> Neighbours8
    {
        get
        {
            yield return new Point2(x - 1, y - 1);
            yield return new Point2(x - 1, y);
            yield return new Point2(x - 1, y + 1);
            yield return new Point2(x, y - 1);
            yield return new Point2(x, y + 1);
            yield return new Point2(x + 1, y - 1);
            yield return new Point2(x + 1, y);
            yield return new Point2(x + 1, y + 1);
        }
    }

    public IEnumerable<Point2> Neighbours4
    {
        get
        {
            yield return new Point2(x - 1, y);
            yield return new Point2(x, y - 1);
            yield return new Point2(x, y + 1);
            yield return new Point2(x + 1, y);
        }
    }    

    public static Point2 operator + (Point2 a, Point2 b) => new Point2(a.x + b.x, a.y + b.y);
    public static Point2 operator - (Point2 a, Point2 b) => new Point2(a.x - b.x, a.y - b.y);
    public static Point2 operator * (Point2 p, int f) => new Point2(p.x * f, p.y * f);
    public static Point2 operator * (int f, Point2 p) => new Point2(p.x * f, p.y * f);

    public static Point2 North = new Point2(0, -1);
    public static Point2 South = new Point2(0, 1);
    public static Point2 East = new Point2(1, 0);
    public static Point2 West = new Point2(-1, 0);

    public static Point2[] CompassDirections = new[] {  South, East, North, West };
    

    public override string ToString()
    {
        return $"({x},{y})";
    }

}

