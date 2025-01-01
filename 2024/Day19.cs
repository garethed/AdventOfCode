using System.Drawing;

namespace AdventOfCode.AoC2024;

class Day19
{
    [Solution]
    [Test(6, testInput)]
    public long Part1(Towels towels)
    {
        return towels.targetDesigns.Count(d => towels.componentMap.CanBuild(d) > 0);        
    }

    [Solution]
    [Test(16, testInput)]
    public long Part2(Towels towels)
    {
        return towels.targetDesigns.Sum(d => towels.componentMap.CanBuild(d));        
    }


    public class ComponentMap
    {
        bool IsFullComponent = false;
        Dictionary<string,long> memoized = [];

        LazyDictionary<char, ComponentMap> NextComponentMaps = new LazyDictionary<char,ComponentMap>(c => new ComponentMap());

        public void Add(string component, int fromIndex)
        {
            var next = component[fromIndex];
            if (fromIndex == component.Length - 1)
            {
                NextComponentMaps[next].IsFullComponent = true;
            }
            else
            {
                NextComponentMaps[next].Add(component, fromIndex + 1);
            }        
        }

        public long CanBuild(string targetDesign)
        {
            return CanBuild(targetDesign, 0, this);
        }

        long CanBuild(string targetDesign, int fromIndex, ComponentMap root)
        {
            if (memoized.ContainsKey(targetDesign.Substring(fromIndex)))
            {
                return memoized[targetDesign.Substring(fromIndex)];
            }

            var count = 0L;

            if (fromIndex == targetDesign.Length)
            {
                return IsFullComponent ? 1 : 0;
            }
            else 
            {
                var nextChar = targetDesign[fromIndex];

                if (NextComponentMaps.ContainsKey(nextChar))
                {                    
                    count += NextComponentMaps[nextChar].CanBuild(targetDesign, fromIndex + 1, root);
                }
                if (IsFullComponent)                
                {
                    count += root.CanBuild(targetDesign, fromIndex, root);
                }
            }
            memoized[targetDesign.Substring(fromIndex)] = count;
            return count;
        }
    }

    public class Towels
    {
        public List<string> components;
        public List<string> targetDesigns;

        public ComponentMap componentMap = new ComponentMap();

        public Towels(string data)
        {
            var parts = Utils.SplitOnBlankLines(data);
            components = parts[0].Split(", ").OrderByDescending(c => c.Length).ToList();
            targetDesigns = parts[1].TrimEnd('\n').Split("\n").ToList();

            foreach (var c in components)
            {
                componentMap.Add(c, 0);
            }
        }
    }

    private const string testInput = @"r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb";
}