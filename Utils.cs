
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class Utils
{
    public static bool Assert(object input, object actual, object expected)  
    {
        var success = false;

        if ((actual != null) == (expected != null)
            && actual.GetType() == expected.GetType())
        {
            if (actual is System.Collections.IStructuralEquatable equatable)
            {
                success = equatable.Equals(expected, StructuralComparisons.StructuralEqualityComparer);
            }
            else
            {
                success = actual.Equals(expected);

            }
        }
                
        if (success)
        {
            Write("OK: " + describe(actual), ConsoleColor.DarkGreen);
        }
        else
        {
            Write("WRONG: " + describe(actual), ConsoleColor.DarkRed);
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
            WriteLine("  Should be " + describe(expected), ConsoleColor.DarkRed);
        }

        return success;
    }

    public static string describe(object o)
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
            Console.Write(msg + "           ");
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

    // Returns an array with one string for each regex group
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

    public static string After(this string s, string infix)
    {
        return s.Split(infix)[1].Trim();
    }

    public static int[] ToIntArray(this string s, string separator = " ")
    {
        return s.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(n => int.Parse(n)).ToArray();
    }

    public static long[] ToLongArray(this string s, string separator = " ")
    {
        return s.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(n => long.Parse(n)).ToArray();
    }    

    public static void Add<K,V>(this IDictionary<K,HashSet<V>> dict, K key, V value) 
    {
        if (!dict.ContainsKey(key)) {
            dict[key] = new HashSet<V>();
        }
        
        dict[key].Add(value);
    }

    public static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }    

    public static IEnumerable<V> Pairwise<T,V>(this IEnumerable<T> seq, Func<T,T,V> func)
    {
        return seq.Zip(seq.Skip(1), func);
    }

    public static string[] SplitOnBlankLines(string input)
    {
        return input.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
    }

    public static IEnumerable<(T First, T Second)> GetUniquePairs<T>(this IEnumerable<T> source)
    {
        var items = source.ToList();        
        return items
            .SelectMany((item, index) => 
                items.Skip(index + 1)
                    .Select(nextItem => (item, nextItem)));
    }    

    
}