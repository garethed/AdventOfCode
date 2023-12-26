using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public record Solution(int year, int day, int part, Action action)
{
    public void Invoke() => action();

    public static List<Solution> AllSolutions => SolutionAttribute.GetAllSolutions();
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SolutionAttribute : Attribute
{
    public int year { get; private set; }
    public int day { get; private set; }
    public int part { get; private set; }

    public object[] additionalInputs;

    private object instance;

    public SolutionAttribute(int year, int day, int part, params object[] additionalInputs)
    {
        this.year = year;
        this.day = day;
        this.part = part;
        this.additionalInputs = additionalInputs;
    }

    void InvokeMethod(MethodInfo methodInfo)
    {
        if (instance == null) 
        {
            instance = Activator.CreateInstance(methodInfo.DeclaringType);      
            TestAttribute.TestAnnotatedMethods(instance);
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
        Utils.Write(methodInfo.Invoke(instance, parameters).ToString(), ConsoleColor.White);
        var time = (DateTime.Now - before).TotalMilliseconds;
        Utils.WriteLine($" ({time:F1}ms)", ConsoleColor.DarkGray);
    }

    internal static List<Solution> GetAllSolutions()
    {
        return Assembly
        .GetCallingAssembly()
        .GetTypes()
        .SelectMany( t => t.GetMethods())
        .SelectMany(m => m.GetCustomAttributes(typeof(SolutionAttribute), false ).Cast<SolutionAttribute>().Select(a => new { Method = m, SolutionAttribute = a}))
        .Select(t => new Solution(t.SolutionAttribute!.year, t.SolutionAttribute.day, t.SolutionAttribute.part, () => t.SolutionAttribute.InvokeMethod(t.Method)))
        .OrderByDescending(s => s.year).ThenByDescending(s => s.day).ThenByDescending(s => s.part)
        .ToList();
    }
}