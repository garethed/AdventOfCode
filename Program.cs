global using AdventOfCode;
using System.Data.Common;

var solutions = Solution.AllSolutions;

if (args.Length == 0)
{
    solutions.First().Invoke();
}
else if (args.Length == 1)
{
    solutions.Where(s => s.year == int.Parse(args[0])).First().Invoke();
}
else
{
    var year = int.Parse(args[0]);
    var yearSolutions = solutions.Where(s => s.year == year).OrderBy(s => s.day).ThenBy(s => s.part).ToList();
    
    foreach (var solution in yearSolutions )
    {
        solution.Invoke();
    }

    Console.WriteLine();
    Utils.WriteLine($"*** {year} ***", ConsoleColor.Red);

    for (int i = 0; i < yearSolutions.Count(); i++)
    {
        var solution = yearSolutions[i];

        Utils.Write($"Day {solution.day:00}", ConsoleColor.Red);
        Utils.Write(" | ", ConsoleColor.White);
        if (solution.TestsPassed)
        {
            Utils.Write($"Tests passed ", ConsoleColor.Green);            
        }        
        else
        {
            Utils.Write($"Tests FAILED ", ConsoleColor.Red);
        }

        Utils.Write($" {solution.TestTime,8:0.0}ms", ConsoleColor.Gray);

        if (solution.part == 1)
        {
            Utils.Write($" | Part 1 ", ConsoleColor.White);
            Utils.Write(string.Format("{0,24}", solution.SolutionOutput), ConsoleColor.Cyan);
            Utils.Write($" {solution.SolutionTime,8:0.0}ms", ConsoleColor.Gray);
            if (i < yearSolutions.Count - 1 && yearSolutions[i + 1].day == solution.day) 
            {
                i++;
                solution = yearSolutions[i];
            }
        }

        if (solution.part == 2)
        {
            Utils.Write($" | Part 2 ", ConsoleColor.White);
            Utils.Write(string.Format("{0,24}", solution.SolutionOutput.Substring(0,Math.Min(24, solution.SolutionOutput.Length))), ConsoleColor.Cyan);
            Utils.Write($" {solution.SolutionTime,8:0.0}ms", ConsoleColor.Gray);
        }
        Utils.WriteLine(" | ", ConsoleColor.White);        
    }

        Utils.Write("Total Time ", ConsoleColor.Red);
        Utils.WriteLine($" {yearSolutions.Sum(s => s.SolutionTime),8:0.0}ms", ConsoleColor.Gray);
}
