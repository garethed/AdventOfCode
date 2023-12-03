namespace AdventOfCode.AoC2015;

public class Day11
{

    [Solution(2015,11,1)]
    [Test("abcdffaa", "abcdefgh")]
    public string NextValidPassword(string previousPassword)
    {
        var chars = previousPassword.ToCharArray();
        IncrementString(chars);

        while (!HasStraight(chars) || !NoInvalidLetters(chars) || !TwoPairs(chars))
        {
            IncrementString(chars);
        }

        return new string(chars);
    }

    [Solution(2015,11,2)]
    public string NextButOneValidPassword(string previousPassword)
    {
        return NextValidPassword(NextValidPassword(previousPassword));
    }


    private bool HasStraight(char[] input)
    {
        return input.Windowed(3).Where(t => t[0] + 1 == t[1] && t[1] + 1 == t[2]).Any();
    }

    private bool NoInvalidLetters(char[] input)
    {
        return input.All(c => c != 'i' && c != 'o' && c != 'l');
    }

    private bool TwoPairs(char[] input)
    {
        return Enumerable.Range(0, input.Length - 1)
        .Where(i => input[i] == input[i + 1] && (i == 0 || input[i] != input[i - 1]))
        .Count() > 1;
    }

    private void IncrementString(char[] input)
    {
        void IncrementChar(char[] input, int index)
        {
            if (input[index] == 'z')
            {
                input[index] = 'a';
                IncrementChar(input, index - 1);
            }
            else 
            {
                input[index] = (char)(input[index] + 1);
            }
        }

        IncrementChar(input, input.Length - 1);
    }
}