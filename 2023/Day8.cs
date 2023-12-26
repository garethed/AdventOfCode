using System.Collections;

namespace AdventOfCode.AoC2023;

class Day8
{
    [Solution(2023,8,1)]
    [Test(6, testData)]
    public long Part1(string[] input)
    {
        var steps = input[0];
        var nodes = buildNodes(input.Skip(2).ToArray());

        var current = nodes["AAA"];
        var count = 0;        
        
        while (true)
        {
            current = steps[count % steps.Length] switch 
            {
                'L' => current.Left,
                'R' => current.Right,
                _ => throw new InvalidOperationException()
            };
            count++;

            if (current.Id == "ZZZ")
            {
                return count;
            }
        }
    }

    [Solution(2023,8,2)]
    [Test(6, testData2)]
    public long Part2(string[] input)    
    {
        var steps = input[0];
        var nodes = buildNodes(input.Skip(2).ToArray());
        
        var nodesToProcess = nodes.Values.Where(n => n.Id.EndsWith("A")).ToArray();
        var zIndexes = nodesToProcess.Select(n => new List<long>()).ToArray();
        var zDeltas = nodesToProcess.Select(n => new List<long>()).ToArray();

        var count = 0;        
        
        while (true)
        {
            bool allEndZ = true;

            for (int i = 0; i < nodesToProcess.Length; i++)
            {
                nodesToProcess[i] = steps[count % steps.Length] switch 
                {
                    'L' => nodesToProcess[i].Left,
                    'R' => nodesToProcess[i].Right,
                    _ => throw new InvalidOperationException()
                };

                allEndZ &= nodesToProcess[i].Id.EndsWith("Z");
                if (nodesToProcess[i].Id.EndsWith("Z"))
                {
                    zIndexes[i].Add(count + 1);
                    if (zIndexes[i].Count > 1)
                    {
                        zDeltas[i].Add(zIndexes[i][zIndexes[i].Count - 1] - zIndexes[i][zIndexes[i].Count - 2]);
                    }
                }
            }

            count++;

            if (count == 100000)
            {
                continue;
            }

            
            if (allEndZ)
            {
                return count;
            }

            if (zIndexes.All(z => z.Count > 0))
            {
                break;
            }
        }

        var lcm = 1L;
        foreach (var z in zIndexes)
        {
            lcm = Utils.lcm(lcm, z.First());
        }

        return lcm;

    }

    private Dictionary<string, Node> buildNodes(string[] input)
    {
        var nodes = new Dictionary<string,Node>();

        foreach (var l in input)
        {
            var parts = l.Split(" =(,)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var node = getNode(parts[0]);
            node.Left = getNode(parts[1]);
            node.Right = getNode(parts[2]);
        }

        return nodes;

        Node getNode(string id)
        {
            if (!nodes.ContainsKey(id))
            {
                nodes[id] = new Node(id);
            }

            return nodes[id];
        }
    }

    public record Node(string Id)
    {
        public Node? Left;
        public Node? Right;        
    }

    private const string testData = @"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
";

    private const string testData2 = @"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
";

}