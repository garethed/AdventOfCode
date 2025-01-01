namespace AdventOfCode.AoC2023;

public class Day25
{
    [Solution(2023,25,1)]
    [Test(54, testData)]
    public int Part1(string[] input)
    {
        var nodes = new Dictionary<string, Node>();
        var edges = new List<string>();

        foreach (var line in input)
        {
            var parts = line.Replace(":", "").Split(' ');
            var sourceNode = getNode(parts[0]);
            foreach (var targetNode in parts.Skip(1).Select(p => getNode(p)))
            {
                var edgeName = sourceNode.Id + "/" + targetNode.Id;
                sourceNode.ConnectedNodes[targetNode] = edgeName;
                targetNode.ConnectedNodes[sourceNode] = edgeName;
                edges.Add(edgeName);
            }
        }

        var excludedEdges = new List<string>();

        while (true)
        {
            var baseline = sumDistances(excludedEdges, "-", out _);            
            var edgeCosts = edges.Select(e => new { e, cost = sumDistances(excludedEdges, e, out _) - baseline}).OrderByDescending(e => e.cost).ToList();

            if (edgeCosts.Last().cost < 0)
            {
                 excludedEdges.Add(edgeCosts.Last().e);
                 break;
            }
            else
            {
                excludedEdges.Add(edgeCosts.First().e);            
            }
        }
        
        int subgroupSize;
        sumDistances(excludedEdges, "-", out subgroupSize);
        return subgroupSize * (nodes.Count - subgroupSize);

        int sumDistances(List<string> excludedEdges, string trialExclusion, out int reachableNodeCount)
        {
            var distances = new Dictionary<Node,int>();
            var queue = new Queue<Node>();
            queue.Enqueue(nodes.First().Value);
            distances[queue.Peek()] = 0;
            int sumOfDistances = 0;

            while (queue.Any())
            {
                var nextNode = queue.Dequeue();
                foreach (var connectedNode in nextNode.ConnectedNodes)
                {
                    if (!distances.ContainsKey(connectedNode.Key) && !excludedEdges.Contains(connectedNode.Value) && trialExclusion != connectedNode.Value)
                    {
                        var d = distances[nextNode];
                        distances[connectedNode.Key] = d + 1;
                        queue.Enqueue(connectedNode.Key);
                        sumOfDistances += d + 1;
                    }
                }            
            }

            reachableNodeCount = distances.Count;

            return sumOfDistances;
        }

        return 0;





        Node getNode(string id)
        {
            var node = nodes.GetOrConstruct(id); 
            node.Id = id;
            return node;
        } 

    }

    public class Node
    {
        public string Id;
        public Dictionary<Node, string> ConnectedNodes = new();
    }

    public const string testData = @"jqt: rhn xhk nvd
rsh: frs pzl lsr
xhk: hfx
cmg: qnr nvd lhk bvb
rhn: xhk bvb hfx
bvb: xhk hfx
pzl: lsr hfx nvd
qnr: nvd
ntq: jqt hfx bvb xhk
nvd: lhk
lsr: lhk
rzs: qnr cmg lsr rsh
frs: qnr lhk lsr
";
}