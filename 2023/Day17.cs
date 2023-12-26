using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.AoC2023;

public class Day17
{
    [Solution(2023,17,1)]
    [Test(102, testData)]
    public long Part1(CharGrid2 input)
    {
        var map = input.Cast<int>(c => int.Parse(c.ToString()));
        var queue = new Queue<PathState>();
        var pathCosts = new Dictionary<PathState, int>();
        var exitCost = int.MaxValue;
        var exit = new Point2(map.Width - 1, map.Height - 1);

        var startState = new PathState(new Point2(0,0), new Point2(0,0), 0);
        pathCosts[startState] = 0;
        queue.Enqueue(startState);

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var currentCost = pathCosts[next];
            if (currentCost > exitCost) continue;

            foreach (var direction in Point2.CompassDirections)
            {
                if (direction != -1 * next.direction && (direction != next.direction || next.continuousSteps < 3))
                {
                    var newState = new PathState(next.location + direction, direction, (direction == next.direction ? next.continuousSteps + 1 : 1));
                    if (!map.Contains(newState.location)) continue;

                    var newCost = currentCost + map[newState.location];
                    if (!pathCosts.ContainsKey(newState) || pathCosts[newState] > newCost)
                    {
                        pathCosts[newState] = newCost;
                        queue.Enqueue(newState);

                        if (newState.location == exit && newCost < exitCost)
                        {
                            exitCost = newCost;
                        }
                    }                
                }
            }
        }

        return exitCost;
    }

    [Solution(2023,17,2)]
    [Test(94, testData)]
    public long Part2(CharGrid2 input)
    {
        var map = input.Cast<int>(c => int.Parse(c.ToString()));
        var pathCosts = new Dictionary<PathState, int>();
        var queue = new Queue<PathState>();
        var exitCost = int.MaxValue;
        var exit = new Point2(map.Width - 1, map.Height - 1);

        var startState = new PathState(new Point2(0,0), new Point2(0,0), -1);
        pathCosts[startState] = 0;
        queue.Enqueue(startState);        

        while (queue.Any())
        {
            var next = queue.Dequeue();
            var currentCost = pathCosts[next];
            if (currentCost > exitCost) continue;

            foreach (var direction in Point2.CompassDirections)
            {
                if (direction != -1 * next.direction 
                    && ((direction == next.direction && next.continuousSteps < 10) || (direction != next.direction && (next.continuousSteps >= 4 || next.continuousSteps == -1))))
                {
                    var newState = new PathState(next.location + direction, direction, (direction == next.direction ? next.continuousSteps + 1 : 1));
                    if (!map.Contains(newState.location)) continue;

                    var newCost = currentCost + map[newState.location];
                    if (!pathCosts.ContainsKey(newState) || pathCosts[newState] > newCost)
                    {
                        pathCosts[newState] = newCost;
                        queue.Enqueue(newState);

                        if (newState.location == exit && newState.continuousSteps >= 4 && newCost < exitCost)
                        {
                            exitCost = newCost;
                        }
                    }                
                }
            }
        }

        return exitCost;
    }    

    private record PathState(Point2 location, Point2 direction, int continuousSteps) {}

    private const string testData = @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533
";
}