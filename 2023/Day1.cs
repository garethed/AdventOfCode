namespace AdventOfCode.AoC2023;

class Day1 
{
    [Solution(2023,1,1)]
    [Test(142, testData)]
    public int Part1(string[] input)
    {
        return input.Sum(l => firstAndLast(l));


        int firstAndLast(string line) 
        {
            return int.Parse(
                line.First(c => char.IsDigit(c)).ToString()
                + line.Last(c => char.IsDigit(c)));
        }

    }

    [Solution(2023,1,2)]
    [Test(281, testData2)]
    public int Part2(string[] input)
    {
        return Part1(input.Select(l => convertDigits(l)).ToArray());

        string convertDigits(string line)
        {
            return line
            .Replace("one", "o1e")
            .Replace("two", "t2o")
            .Replace("three", "t3e")
            .Replace("four", "f4r")
            .Replace("five", "f5e")
            .Replace("six", "s6x")
            .Replace("seven", "s7n")
            .Replace("eight", "e8t")
            .Replace("nine", "n9e")            ;

        }
    }

    private const string testData = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

    private const string testData2 = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";
}
