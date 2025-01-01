using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public record Solution(int year, int day, int part, Action<Solution> action)
{
    public void Invoke() => action(this);

    public bool TestsPassed;
    public double TestTime;
    public double SolutionTime;
    public string SolutionOutput;
    public static List<Solution> AllSolutions => SolutionAttribute.GetAllSolutions();
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SolutionAttribute : Attribute
{
    public int year { get; set; }
    public int day { get; set; }
    public int part { get; set; }

    public object[] additionalInputs;

    private object instance;

    public SolutionAttribute()
    {

    }

    public SolutionAttribute(int year, int day, int part, params object[] additionalInputs)
    {
        this.year = year;
        this.day = day;
        this.part = part;
        this.additionalInputs = additionalInputs;
    }

    void InvokeMethod(MethodInfo methodInfo, Solution solution)
    {
        if (instance == null) 
        {
            instance = Activator.CreateInstance(methodInfo.DeclaringType);      
            var beforeTests = DateTime.Now;
            solution.TestsPassed = TestAttribute.TestAnnotatedMethods(instance);
            solution.TestTime = (DateTime.Now - beforeTests).TotalMilliseconds;
        }

        var parameters = new object[methodInfo.GetParameters().Length];
        if (parameters.Length > 0) 
        {
            parameters[0] = Input.Get(year,day).GetTyped(methodInfo.GetParameters().First().ParameterType);
        }
        if (parameters.Length > 1)
        {
            additionalInputs.CopyTo(parameters, 1);
        }

        Utils.Write($"{year} day {day} part {part}: ", ConsoleColor.Magenta); 
        var before = DateTime.Now;
        var output = Utils.describe(methodInfo.Invoke(instance, parameters));
        Utils.Write(output, ConsoleColor.White);
        var time = (DateTime.Now - before).TotalMilliseconds;
        Utils.WriteLine($" ({time:F1}ms)", ConsoleColor.DarkGray);

        solution.SolutionOutput = output;
        solution.SolutionTime = time;
    }

    public Solution MakeSolution(MethodInfo methodInfo)
    {
        if (year == 0 || day == 0 || part == 0)
        {
            InferDate(methodInfo);
        }

        return new Solution(year, day, part, s => InvokeMethod(methodInfo, s));
    }

    private void InferDate(MethodInfo methodInfo)
    {
        if (year == 0)
        {
            var namespaceName = methodInfo.DeclaringType.Namespace;
            year = int.Parse(namespaceName.Substring(namespaceName.Length - 4, 4));
        }
        if (day == 0)
        {
            var typeName = methodInfo.DeclaringType.Name;
            day = int.Parse(typeName.Replace("Day", ""));
        }
        if (part == 0)
        {
            part = int.Parse(methodInfo.Name.Last().ToString());    
        }
    }

    internal static List<Solution> GetAllSolutions()
    {
        return Assembly
        .GetCallingAssembly()
        .GetTypes()
        .SelectMany( t => t.GetMethods())
        .SelectMany(m => m.GetCustomAttributes(typeof(SolutionAttribute), false ).Cast<SolutionAttribute>().Select(a => a.MakeSolution(m)))
        .OrderByDescending(s => s.year).ThenByDescending(s => s.day).ThenByDescending(s => s.part)
        .ToList();
    }
}