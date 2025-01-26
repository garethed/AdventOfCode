using System.Collections;

namespace AdventOfCode.AoC2017;

public class Day21
{
    [Solution(2017,21,1, additionalInputs = [5])]
    [Solution(2017,21,2, additionalInputs = [18])]
    [Test(12, testData, 2)]
    public long Part1(Rules input, int iterations)
    {
        var grid = new BitArray([Square.Parse(".#./..#/###").Data]);        
        var size = 3;

        /*foreach (var s in Square.Parse(".#./..#/###").generateVariants())
        {
            print(new BitArray([s.Data]), s.Size);
        }*/

        for (var i = 0; i < iterations; i++)
        {
            var divisor = (size % 2 == 0) ? 2 : 3;            
            var newSize = (size % 2 == 0) ? size / 2 * 3 : size / 3 * 4;
            var next = new BitArray(newSize * newSize);

            //print(grid, size);

            for (var y = 0; y < size / divisor; y++)
            {
                for (var x = 0; x < size / divisor; x++)
                {
                    var s = get(grid, size, x, y, divisor);
                    var s2 = input.transformations[s];
                    put(next, newSize, x, y, s2);
                }
            }

            grid = next;
            size = newSize;                
        }

        //print(grid, size);

        return grid.Cast<bool>().Count(x => x);

        Square get(BitArray a, int sa, int x, int y, int s)
        {
            var i = 0;

            for (int y2 = s - 1; y2 >= 0; y2--)
            {                
                for (int x2 = s - 1; x2 >= 0; x2--)
                {
                    i <<= 1;
                    if (a[(y * s + y2) * sa + x * s + x2])
                    {
                        i++;
                    }
                }
            }

            return new Square(s, i);

        }

        void put(BitArray a, int sa, int x, int y, Square s)
        {
            var i = s.Data;

            for (int y2 = 0; y2 < s.Size; y2++)
            {                
                for (int x2 = 0; x2 < s.Size; x2++)
                {
                    if ((i & 1) == 1)
                    {
                        a.Set((y * s.Size + y2) * sa + x * s.Size + x2, true);
                    }

                    i >>= 1;                    
                }
            }
        }

        void print(BitArray a, int s)
        {
            Console.WriteLine();

            for (int y2 = 0; y2 < s; y2++)
            {                
                for (int x2 = 0; x2 < s; x2++)
                {
                    Console.Write(a[y2 * s + x2] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
    }


    public class Rules
    {
        public Dictionary<Square, Square> transformations = [];

        public Rules(string[] input)
        {
            foreach (var rule in input)
            {
                var parts = rule.Split(" => ", 2).Select(Square.Parse).ToArray();
                foreach (var variant in parts[0].generateVariants())
                {
                    transformations[variant] = parts[1];
                }                
            }
        }
    }

    public record Square(int Size, int Data)
    {
        public static Square Parse(string input)
        {
            var s = 1;
            var d = 0;

            foreach (var c in input.Reverse())
            {
                if (c != '/')
                {
                    d <<= 1;
                }
                else 
                {
                    s++;
                }

                if (c == '#')
                {
                    d++;
                }
            }

            return new Square(s, d);
        }

        public IEnumerable<Square> generateVariants()
        {            
            var s = Data;
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
            s = Reflect(Data);
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
            s = Rotate(s);
            yield return new Square(Size, s);
        
            int Rotate(int i)
            {
                BitArray bi = new BitArray([i]);
                BitArray bo = new BitArray(32); 
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        bo.Set(Size * (Size - x - 1) + y, bi.Get(Size * y + x));                        
                    }
                }

                var a = new int[1];
                bo.CopyTo(a, 0);
                return a[0];
            }

            int Reflect(int i)
            {
                BitArray bi = new BitArray([i]);
                BitArray bo = new BitArray(32); 
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        bo.Set(Size * x + y, bi.Get(Size * y + x));                        
                    }
                }

                var a = new int[1];
                bo.CopyTo(a, 0);
                return a[0];

            }
        }

    }

    private const string testData = @"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#";
}