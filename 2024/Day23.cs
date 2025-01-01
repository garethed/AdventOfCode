namespace AdventOfCode.AoC2024;

class Day23
{
    [Solution]
    [Test(7, testInput)]
    public long Part1(Network network)
    {
        var count = 0;

        foreach (var node in network.Nodes.Values)
        {
            foreach (var node2 in node.neighbours.Where(n => n.id.CompareTo(node.id) > 0))
            {
                foreach (var node3 in node2.neighbours.Where( n => n.neighbours.Contains(node) && n.id.CompareTo(node2.id) > 0))
                {
                    if (node.id.StartsWith("t") || node2.id.StartsWith("t") || node3.id.StartsWith("t"))
                    {
                        count++;
                    }
                }

            }

        }

        return count;
    }

    [Solution]
    [Test("co,de,ka,ta", testInput)]
    public string Part2(Network network)
    {
        var maxSetSize = 0;
        var password = "";

        foreach (var node in network.Nodes.Values)
        {
            recurseFindSet(new Stack<Node>( [node]));        
        }

        return password;

        void recurseFindSet(Stack<Node> nodesSoFar)
        {
            if (nodesSoFar.Count > maxSetSize)
            {
                maxSetSize = nodesSoFar.Count;
                password = String.Join(',', nodesSoFar.Select(n => n.id).OrderBy(i => i));
            }

            var lastNode = nodesSoFar.Peek();
            foreach (var node in lastNode.neighbours.Where(n => n.id.CompareTo(lastNode.id) > 0))
            {
                if (nodesSoFar.All(n => n.neighbours.Contains(node)))
                {
                    nodesSoFar.Push(node);
                    recurseFindSet(nodesSoFar);
                    nodesSoFar.Pop();
                }
            }
        }
    }

    public class Network
    {
        public LazyDictionary<string, Node> Nodes = new LazyDictionary<string, Node>(id => new Node() { id = id });

        public Network(string[] input)
        {            
            foreach (var line in input)
            {
                var parts = line.Split('-');
                var n1 = Nodes[parts[0]];
                var n2 = Nodes[parts[1]];
                n1.neighbours.Add(n2);
                n2.neighbours.Add(n1);
            }

        }
    }

    public class Node() 
    {
        public string id;
        public HashSet<Node> neighbours = [];
    }


    private const string testInput = @"kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn";
}
