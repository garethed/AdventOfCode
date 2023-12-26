namespace AdventOfCode;

public struct Rect
{
    public Point2 topLeft;
    public Point2 bottomRight;

    public long Area
    {
        get => (long)(bottomRight.x - topLeft.x) * (bottomRight.y - topLeft.y);
    }

    public Rect(Point2 p1, Point2 p2)
    {
        topLeft = new Point2(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y));
        bottomRight = new Point2(Math.Max(p1.x, p2.x), Math.Max(p1.y, p2.y));
    }

    public Rect(Point2 p1, int w, int h)
    {
        topLeft = p1;
        bottomRight = new Point2(p1.x + w, p1.y + h);
    }


    public Rect(int x, int y, int w, int h)
    {
        topLeft = new Point2(x,y);
        bottomRight = new Point2(x + w, y + h);
    }

    public bool Contains(Rect other)
    {
        return topLeft.x <= other.topLeft.x && topLeft.y <= other.topLeft.y && bottomRight.x >= other.bottomRight.x && bottomRight.y >= other.bottomRight.y;
    }

    public int Width { get => bottomRight.x - topLeft.x; }
    public int Height { get => bottomRight.y - topLeft.y; }

    public Rect LeftEdge { get => new Rect(topLeft, 0, Height); }
    public Rect RightEdge { get => new Rect(bottomRight.x, topLeft.y, 0, Height); }

    public Rect TopEdge { get => new Rect(topLeft, Width, 0); }
    public Rect BottomEdge { get => new Rect(topLeft.x, bottomRight.y, Width, 0); }


    
}