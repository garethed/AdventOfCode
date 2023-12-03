namespace AdventOfCode.AoC2015;

public class Day9
{


    [Solution(2015,9,1)]
    [Test(605, testData)]
    public int ShortestTrip(string[] input) => FindTrip(input, true);
    [Solution(2015,9,2)]
    [Test(982, testData)]
    public int LongestTrip(string[] input) => FindTrip(input, false);


    public int FindTrip(string[] input, bool shortest)
    {
        var multiplier = shortest ? 1 : -1;
        var distances = new Dictionary<string,int>();
        var labels = new Dictionary<string,char>();
        var nextLabel = 'a';
        var allLabels = "";
        char getLabel(string name) 
        {
            if (labels.ContainsKey(name)) return labels[name];
            labels[name] = nextLabel;   
            allLabels += nextLabel;         
            return nextLabel++;
        }

        foreach (var s in input)
        {
            // London to Dublin = 464
            var parts = s.Split(' ');
            var from = getLabel(parts[0]);
            var to = getLabel(parts[2]);
            var distance = int.Parse(parts[4]);
            distances[from.ToString() + to] = distance * multiplier;
            distances[to.ToString() + from] = distance * multiplier;                        
        }

        return allLabels
            .Select(c => distance(c + allLabels.Replace(c.ToString(), ""), distances))
            .Min() * multiplier;        
    }

    private int distance(string tree, Dictionary<string, int> distances)
    {
        if (distances.ContainsKey(tree)) return distances[tree];

        var unvisited = tree.Substring(1);
        var shortest = unvisited.Select( c =>         
            {
                var subtree = c + unvisited.Replace(c.ToString(), "");
                return distances[tree[0].ToString() + c] + distance(subtree, distances);
            }).Min();

        distances[tree] = shortest;
        return shortest;
    }

    const string testData = @"London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141";
}