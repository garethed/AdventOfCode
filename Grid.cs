using System.Drawing;
using System.Text;

namespace AdventOfCode;

public class Grid2<T>
{
    public T[,] Data;
    public int Width;
    public int Height;

    public Grid2(T[,] Data) 
    { 
        this.Data = Data; 
        Width = Data.GetLength(0);
        Height = Data.GetLength(1);
    }

    public T this[Point2 p]
    {
        get { return Data[p.x, p.y]; }
        set { Data[p.x, p.y] = value; }
    }

    public T this[int x, int y]
    {
        get { return Data[x, y]; }
        set { Data[x, y] = value; }
    }

    public bool Contains(Point2 p)
    {
        return p.x >= 0 && p.x < Width && p.y >= 0 && p.y < Height;
    }

    public IEnumerable<Point2> Points { get { return Point2.enumerateGrid(Width, Height); }}

    public IEnumerable<Point2> ContainedPoints(IEnumerable<Point2> input) { return input.Where(p => Contains(p)); }

    public IEnumerable<Point2> Neighbours8(Point2 p) { return ContainedPoints(p.Neighbours8); }

    public override string ToString()
    {
        return $"{typeof(T).Name}[{Width}x{Height}]";
    }

}

public class CharGrid2 : Grid2<char>
{
    public CharGrid2(String[] Data) : base(CharArrayFromStrings(Data)) {}
    public CharGrid2(string data) : this(Utils.splitLines(data)) {}

    private static char[,] CharArrayFromStrings(string[] strings)
    {
        var ret = new char[strings[0].Length, strings.Length];
        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[0].Length; x++)
            {
                ret[x,y] = strings[y][x];
            }
        }

        return ret;
    }

    public Grid2<T> Cast<T>(Func<char,T> func)
    {
        var converted = new T[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                converted[x,y] = func(Data[x,y]);
            }
        }

        return new Grid2<T>(converted);
    }

    public void Print()
    {
        Console.Write("\n\n");
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Utils.Write(Data[x,y].ToString(), ConsoleColor.Gray);
            }
            Console.WriteLine();
        }
    }

    public string getString()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                sb.Append(Data[x,y]);
            }
        }
        return sb.ToString();
    }    



}