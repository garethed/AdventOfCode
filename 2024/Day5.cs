namespace AdventOfCode.AoC2024;

class Day5
{
    [Solution]
    [Test(143, testInput)]
    public int Part1(PrinterData input)
    {
        return input.PageLists.Where(input.isValid).Sum(p => p[p.Count / 2]);
    }

    [Solution]
    [Test(123, testInput)]
    public int Part2(PrinterData input)
    {
        return input.PageLists
        .Where(l => !input.isValid(l))
        .Select(sortList)
        .Sum(p => p[p.Count / 2]);

        List<int> sortList(List<int> list)
        {
            list.Sort(new ListComparer(list, input));
            return list;            
        }
    }

    class ListComparer(List<int> allowedValues, PrinterData input) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == y) return 0;
            return input.Successors[x].Contains(y) ? -1 : 1;
        }        
    }

    public class PrinterData
    {
        public ListValuedDictionary<int,int> Predecessors = new ListValuedDictionary<int, int>();
        public ListValuedDictionary<int,int> Successors = new ListValuedDictionary<int, int>();

        public List<List<int>> PageLists;

        public PrinterData(string input)
        {
            var parts = Utils.SplitOnBlankLines(input);
            foreach (var line in Utils.splitLines(parts[0]))
            {
                var ints = line.Split('|').Select(x => int.Parse(x)).ToArray();
                Predecessors.Add(ints[1], ints[0]);
                Successors.Add(ints[0], ints[1]);
            }

            PageLists = Utils.splitLines(parts[1])
                .Select(l => l.Split(',').Select(x => int.Parse(x)).ToList()).ToList();            
        }

        public bool isValid(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var next = list[i];
                var predecessors = Predecessors[next];
                if (predecessors.Any(p => list.IndexOf(p, i + 1) >= 0)) return false;
            }

            return true;
        }

        
    }

    private const string testInput = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
";
}
