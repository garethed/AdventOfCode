using System.Drawing;

namespace AdventOfCode;

public record Point2(int x, int y)
{
    public Point2 Dx(int dx) => new Point2(x + dx, y);
    public Point2 Dy(int dy) => new Point2(x, y + dy);

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

}

