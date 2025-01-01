using System.Diagnostics;

namespace AdventOfCode.AoC2023;

public class Day23
{
    [Solution(2023,23,1)]
    [Test(94, testData)]
    public long Part1(CharGrid2 input)
    {
        return maxPath(input, false);
    }

    [Solution(2023,23,2)]
    [Test(154, testData)]
    public long Part2(CharGrid2 input)
    {
        return maxPath(input, true);
    }

    long maxPath(CharGrid2 input, bool ignoreSlopes)
    {

        var nodeCoordinates = input.Points.Where(p => IsDecisionPoint(p)).ToHashSet();
        var startCoord = new Point2(1,0);
        var finishCoord = new Point2(input.Width - 2, input.Height - 1);
        Debug.Assert(input[startCoord] == '.' && input[finishCoord] == '.');
        nodeCoordinates.Add(startCoord);
        nodeCoordinates.Add(finishCoord);
        var nodes = nodeCoordinates.Select(nc => new Node() { p = nc }).ToDictionary(nc => nc.p, nc => nc);
        var start = nodes[startCoord];
        var finish = nodes[finishCoord];

        foreach (var node in nodes.Values)
        {
            InitNode(node);
        }

        var maxPath = 0;
        var path = new HashSet<Node> { start };
        buildPath(path, start, 0);

        return maxPath;

        void buildPath(HashSet<Node> nodes, Node current, int length)
        {
            if (current == finish)
            {
                maxPath = Math.Max(length, maxPath);
                return;
            }

            foreach (var next in current.nextNodes.Where(kv => !nodes.Contains(kv.Key)))
            {
                nodes.Add(next.Key);
                buildPath(nodes, next.Key, length + next.Value);
                nodes.Remove(next.Key);                
            }
        }

        bool IsDecisionPoint(Point2 p)
        {
            return input[p] == '.' && input.Neighbours4(p).Count(n => input[n] == '#') < 2;
        }

        void InitNode(Node node)
        {
            foreach (var n in input.Neighbours4(node.p).Where(n1 => input[n1] != '#'))
            {
                var current = n;
                var prev = node.p;
                var distance = 1;

                while (true)
                {
                    if (nodeCoordinates.Contains(current))
                    {
                        node.nextNodes[nodes[current]] = distance;
                        break;
                    }
                    
                    var cell = input[current];
                    if (!ignoreSlopes && cell != '.'  && (current - prev) != PermittedDirections[cell])
                    {
                        break;
                    }

                    var next = current.Neighbours4.Where(n2 => n2 != prev && input[n2] != '#').First();
                    prev = current;
                    current = next;
                    distance++;                
                }
            }            
        }

    }    

    Dictionary<char,Point2> PermittedDirections = new Dictionary<char, Point2>
    {
        ['>'] = Point2.East,
        ['^'] = Point2.North,
        ['v'] = Point2.South,
        ['<'] = Point2.West
    };

    public class Node
    {
        public Point2 p;
        public Dictionary<Node,int> nextNodes = new Dictionary<Node, int>();

    }

    private const string testData = @"#.#####################
#.......#########...###
#######.#########.#.###
###.....#.>.>.###.#.###
###v#####.#v#.###.#.###
###.>...#.#.#.....#...#
###v###.#.#.#########.#
###...#.#.#.......#...#
#####.#.#.#######.#.###
#.....#.#.#.......#...#
#.#####.#.#.#########v#
#.#...#...#...###...>.#
#.#.#v#######v###.###v#
#...#.>.#...>.>.#.###.#
#####v#.#.###v#.#.###.#
#.....#...#...#.#.#...#
#.#########.###.#.#.###
#...###...#...#...#.###
###.###.#.###v#####v###
#...#...#.#.>.>.#.>.###
#.###.###.#.###.#.#v###
#.....###...###...#...#
#####################.#
";
}