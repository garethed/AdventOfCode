namespace AdventOfCode;

public struct Cuboid
{
    public Point3 MinCorner;
    public Point3 MaxCorner;

    public Cuboid (Point3 p1, Point3 p2)
    {
        MinCorner = new Point3(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y), Math.Min(p1.z, p2.z));
        MaxCorner = new Point3(Math.Max(p1.x, p2.x), Math.Max(p1.y, p2.y), Math.Max(p1.z, p2.z));
    }

    public Cuboid Move(Point3 delta)
    {
        return new Cuboid(MinCorner + delta, MaxCorner + delta);
    }

    public bool Touches(Cuboid other)
    {
        return other.MinCorner.z <= MaxCorner.z && other.MaxCorner.z >= MinCorner.z
        && other.MinCorner.x <= MaxCorner.x && other.MaxCorner.x >= MinCorner.x
        && other.MinCorner.y <= MaxCorner.y && other.MaxCorner.y >= MinCorner.y;
    }
}