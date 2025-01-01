namespace AdventOfCode.AoC2023;

public class Day22
{
    [Solution(2023,22,1)]
    [Test(5, testData)]
    public long Part1(Brick[] input)
    {
        return StackBricks(input).Count(b => !b.Essential);
    }

    private Stack<Brick> StackBricks(Brick[] input)
    {
        var sortedBricks = input.OrderBy(b => b.cuboid.MinCorner.z);
        var placedBricks = new Stack<Brick>();

        foreach (var b in sortedBricks)
        {
            while (b.cuboid.MinCorner.z > 0 && !placedBricks.Any(pb => pb.cuboid.Touches(b.cuboid)))
            {
                b.cuboid = b.cuboid.Move(Point3.Down);
            }

            foreach (var sb in placedBricks.Where(pb => pb.cuboid.Touches(b.cuboid)))
            {
                b.supportedBy.Add(sb);
                sb.supports.Add(b);
            }

            b.cuboid = b.cuboid.Move(Point3.Up);
            placedBricks.Push(b);            

            if (b.supportedBy.Count == 1)
            {
                b.supportedBy.First().Essential = true;
            }            
        }
        return placedBricks;
    }

    [Solution(2023,22,2)]
    [Test(7, testData)]
    public long Part2(Brick[] input)
    {        
        return StackBricks(input).Sum(b => totalDependentBricks(b));

        int totalDependentBricks(Brick b)
        {
            var removedBricks = new HashSet<Brick> { b };
            var nextBricks = new HashSet<Brick>();
            nextBricks.UnionWith(b.supports);

            while (true)
            {
                var dependentBricks = nextBricks.Where(pb => removedBricks.IsSupersetOf(pb.supportedBy)).ToList();
                if (dependentBricks.Count() == 0) break;
                removedBricks.UnionWith(dependentBricks);
                nextBricks.Clear();
                nextBricks.UnionWith(dependentBricks.SelectMany(b => b.supports));
                nextBricks.ExceptWith(removedBricks);
            }

            return removedBricks.Count - 1;

        }
    
    }    

    public class Brick
    {
        public Cuboid cuboid;
        public HashSet<Brick> supportedBy = new HashSet<Brick>();
        public HashSet<Brick> supports = new HashSet<Brick>();

        public bool Essential;

        public Brick(string input)
        {
            var parts = input.Split('~');
            cuboid = new Cuboid(Point3.FromString(parts[0]), Point3.FromString(parts[1]));
        }
    }

    private const string testData = @"1,0,1~1,2,1
0,0,2~2,0,2
0,2,3~2,2,3
0,0,4~0,2,4
2,0,5~2,2,5
0,1,6~2,1,6
1,1,8~1,1,9
";

}
