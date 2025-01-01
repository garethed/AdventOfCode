using System.Text.RegularExpressions;

namespace AdventOfCode.AoC2017;

public class Day20
{
    [Solution]
    [Test(0, testData)]
    public long Part1(Particle[] particles)
    {
        return Array.IndexOf(particles, particles.MinBy(p => p.a.Abs() * 10000 + p.v.Abs()));
    }

    [Solution]
    [Test(1, testData2)]
    public int Part2(Particle[] particles)
    {

        var remaining = new HashSet<Particle>();
        var positions = new HashSet<Point3>();
        var cycles = 0;
        var toRemove = new HashSet<Particle>();

        remaining.UnionWith(particles);

        while (cycles++ < 10000)
        {
            positions.Clear();
            toRemove.Clear();

            foreach (var p in remaining)
            {
                p.tick();
                if (!positions.Add(p.p))
                {
                    toRemove.Add(p);
                }                                
            }

            toRemove.UnionWith(remaining.Where(r => toRemove.Any(tr => tr.p == r.p)));
            remaining.ExceptWith(toRemove);            
        }

        return remaining.Count();



    }

    public class Particle
    {
        public Point3 p;
        public Point3 v;
        public Point3 a;

        public Particle() {}

        public Particle(string input)
        {
            var parts = new Regex("\\<(.*?)\\>").Matches(input).Select(m => m.Groups[1].Value).ToArray();
            p = Point3.FromString(parts[0]);
            v = Point3.FromString(parts[1]);
            a = Point3.FromString(parts[2]);
        }

        public void tick() 
        {
            v += a;
            p += v;
        }
    }

    private const string testData = @"p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>";

private const string testData2 = @"p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>";

}