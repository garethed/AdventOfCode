namespace AdventOfCode.AoC2015;

public class Day5 
{

    [Solution(2015,5,1)]
    public int CountNice(string[] inputs)
    {
        return inputs.Count(s => IsNice(s));
    }

    [Solution(2015,5,2)]
    public int CountNice2(string[] inputs)
    {
        return inputs.Count(s => IsNice2(s));
    }    

    [Test(true, "ugknbfddgicrmopn")]
    [Test(false, "jchzalrnumimnmhp")]
    [Test(false, "haegwjzuvuyypxyu")]
    [Test(false, "dvszwmarrgswjxmb")]
    public bool IsNice(string input)
    {
        return HasThreeVowels(input) && HasDoubleLetter(input) && NoInvalidSubstring(input);
    }

    [Test(true, "qjhvhtzxzqqjkmpb")]
    [Test(true, "xxyxx")]
    [Test(false, "uurcxstgmygtbstg")]
    [Test(false, "ieodomkazucvgmuy")]
    public bool IsNice2(string input)
    {
        return HasTwoLetterRepeat(input) && HasOneLetterRepeat(input);
    }    

    private bool HasTwoLetterRepeat(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
        {
            var chars = input.Substring(i, 2);
            if (input.IndexOf(chars, i + 2) >= 0) return true;
        }

        return false;
    }

    private bool HasOneLetterRepeat(string input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {            
            if (input[i] == input[i + 2]) return true;
        }

        return false;
    }


    private bool HasThreeVowels(string input)
    {
        return input.Where(c => "aeiou".Contains(c)).Take(3).Count() == 3;
    }

    private bool HasDoubleLetter(string input)
    {
        return input.Zip(input.Skip(1), (a,b) => a == b).Any(b => b);
    }

    private bool NoInvalidSubstring(string input)
    {
        return new[] {"ab", "cd", "pq", "xy"}.Select(s => input.IndexOf(s) < 0).All(b => b);
    }
}