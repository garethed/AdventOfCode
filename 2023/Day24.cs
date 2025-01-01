namespace AdventOfCode.AoC2023;

public class Day24
{

    [Solution(2023,24,1,200000000000000,400000000000000)]
    [Test(2, testData, 7, 27)]
    public long Part1(Hailstone[] hailstones, long minBoundary, long maxBoundary)
    {
        var xyProjection = hailstones.Select(h => h.XY).ToArray();

        var crossingCount = 0;

        for (int i = 0; i < xyProjection.Length; i++)
        {
            for (int j = i + 1; j < xyProjection.Length; j++)
            {
                var h1 = xyProjection[i];
                var h2 = xyProjection[j];

                var pos =  h1.Intersect(h2);
                if (pos != null &&                 
                    inRange(pos.x) && 
                    inRange(pos.y) &&
                    inFuture(h1, pos) && 
                    inFuture(h2, pos)
                ) {
                    crossingCount++;
                }                                
            }
        }

        return crossingCount;

        bool inRange (decimal d) => minBoundary <= d && d <= maxBoundary;
        bool inFuture (Hailstone2 h, D2 pos) => Math.Sign(pos.x - h.position.x) == Math.Sign(h.velocity.x);
    }

    [Solution(2023,24,2)]
    [Test(47, testData)]
    public object PartTwo(Hailstone[] hailstones) 
    {        
        var stoneXY = FindOrigin(hailstones.Select(h => h.XY).ToArray());
        var stoneXZ = FindOrigin(hailstones.Select(h => h.XZ).ToArray());
        return Math.Round(stoneXY.x + stoneXY.y + stoneXZ.y);
    }

    D2 FindOrigin(Hailstone2[] hailstones) {

        var s = 500; 
        for (var dx = -s; dx < s; dx++) {
            for (var dy = -s; dy < s; dy++) {

                // p0 and p1 are linearly independent (for me) => stone != null
                var intersection = hailstones[0].ChangeReferenceFrame(dx, dy)
                    .Intersect(hailstones[1].ChangeReferenceFrame(dx,dy));

                if (intersection != null && hailstones.All(p => Hits(p.ChangeReferenceFrame(dx,dy), intersection))) {
                    return intersection;
                }
            }
        }
        throw new Exception();
    }

    bool Hits(Hailstone2 p, D2 pos) {
        var d = (pos.x - p.position.x) * p.velocity.y - (pos.y - p.position.y) * p.velocity.x;
        return Math.Abs(d) < (decimal)0.0001;
    }    

    public class Hailstone
    {
        public D3 position;
        public D3 velocity;

        public Hailstone2 XY => new Hailstone2(new D2(position.x, position.y), new D2(velocity.x, velocity.y));
        public Hailstone2 XZ => new Hailstone2(new D2(position.x, position.z), new D2(velocity.x, velocity.z));

        public Hailstone(string input)
        {
            var parts = input.Split(" @ ");
            position = D3.FromString(parts[0]);
            velocity = D3.FromString(parts[1]);
        }    
    }

    public record Hailstone2(D2 position, D2 velocity) 
    {
        public D2? Intersect(Hailstone2 other) 
        {
            var determinant = velocity.x * other.velocity.y - velocity.y * other.velocity.x;
            
            if (determinant == 0) {
                return null; 
            }
            
            var b0 = velocity.x * position.y - velocity.y * position.x;
            var b1 = other.velocity.x * other.position.y - other.velocity.y * other.position.x;
        
            return new (
                (other.velocity.x * b0 - velocity.x * b1) / determinant,
                (other.velocity.y * b0 - velocity.y * b1) / determinant
            );
        }

        public Hailstone2 ChangeReferenceFrame(decimal dx, decimal dy)
        {
            return new Hailstone2(position, new D2(velocity.x - dx, velocity.y - dy));
        }
    }

    public record D2(decimal x, decimal y) {}

    public record D3(decimal x, decimal y, decimal z) {

        public static D3 FromString(string input)
        {
            var points = input.Split(',', StringSplitOptions.TrimEntries).Select(s => decimal.Parse(s)).ToArray();
            return new D3(points[0], points[1], points[2]);

        }
    };

    private const string testData = @"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3
";
}