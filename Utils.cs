
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class Utils
{

    public static bool Assert<T>(object input, T actual, T expected)  {

        var success = EqualityComparer<T>.Default.Equals(actual, expected);
        if (success)
        {
            Write("OK: " + actual.ToString(), ConsoleColor.DarkGreen);
        }
        else
        {
            Write("WRONG: " + actual.ToString(), ConsoleColor.DarkRed);
            success = false;
        }

        var inputString = describe(input).Replace("\n", " ").Replace("\r", "");        
        if (inputString.Length > 120)
        {
            inputString = inputString.Substring(0, 120) + "...";
        }

        WriteLine(" <- " + inputString, ConsoleColor.White);


        if (!success)
        {
            WriteLine("  Should be " + expected.ToString(), ConsoleColor.DarkRed);
        }

        return success;
    }

    private static string describe(object o)
    {
        if (o is IEnumerable && !(o is string))
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in (IEnumerable)o)
            {
                sb.Append(describe(item)).Append(",");
            }
            sb.Append("]");            
            return sb.ToString();
        }
        
        return o.ToString();

    }

    public static void WriteLine(string msg, ConsoleColor color)
    {
        Write(msg, color);
        Console.WriteLine();
    }


    public static void Write(string msg, ConsoleColor color)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = old;
    }

    public static void ClearLine()
    {
        Console.Write(new string(' ', Console.WindowWidth));
        Console.CursorLeft = 0;
    }

    public static void WriteTransient(string msg)
    {
        //if (!System.Diagnostics.Debugger.IsAttached)
        {
            var pos = Console.CursorLeft;
            Console.Write(msg);
            Console.CursorLeft = pos;
        }
    }    

    public static string SanitizeInput(string input) 
    {
        return input.Replace("\r", "").TrimEnd('\n');

    }

    public static string[] splitLines(string input)
    {
        return SanitizeInput(input).Split('\n').Select(l => l.Trim()).ToArray();
    }        

    static MD5 md5 = System.Security.Cryptography.MD5.Create();

    public static string MD5(string input)
    {
        return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach(T item in enumeration)
        {
            action(item);
        }
    }    

    public static string[] RegexSplitByGroups(this string input, string regex)
    {
        var r = new Regex(regex, RegexOptions.Singleline | RegexOptions.Multiline);
        var matches = r.Matches(input);
        if (matches.Count == 1)
        { 
            return matches.First().Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        }
        else 
        {
            throw new InvalidOperationException();
        }
    }

    public static V GetOrConstruct<K,V>(this IDictionary<K,V> dict, K key) where V : new() 
    {
        if (!dict.ContainsKey(key)) {
            dict[key] = new V();
        }
        return dict[key];
    }

    public static IEnumerable<T[]> Windowed<T>(this IEnumerable<T> source, int window)
    {        
        var buffer = new Queue<T>();
        var enumerator = source.GetEnumerator();

        while (enumerator.MoveNext())
        {
            buffer.Enqueue(enumerator.Current);
            if (buffer.Count == window)
            {
                yield return buffer.ToArray();
                buffer.Dequeue();
            }
        }
    }


}