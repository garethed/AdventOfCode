using System.Collections;

namespace AdventOfCode.AoC2023;

class Day4
{
    [Solution(2023,4,1)]
    [Test(13, testInput)]
    public int Part1(Card[] input)
    {
        return input.Sum(c => c.Score);      
    }

    [Solution(2023,4,2)]
    [Test(30, testInput)]
    public int Part2(Card[] input)
    {
        var extraCards = new int[input.Length];

        for (var i = 0; i < input.Length; i++)
        {
            var matches = input[i].Matches;
            var countOfThisCard = 1 + extraCards[i];

            for (var j = 0; j < matches; j++)
            {
                extraCards[i + j + 1] += countOfThisCard;
            }
        }    

        return input.Length + extraCards.Sum();    
    }    

    public class Card
    {
        public BitArray numbers;
        public BitArray winners;

        public Card(string line)
        {
            var parts = line.Split(": ")[1].Split(" | ");
            winners = parseNumbers(parts[0]);
            numbers = parseNumbers(parts[1]);
        }

        public int Score 
        {
            get
            {
                var matches = numbers.And(winners).OfType<bool>().Count(b => b);
                return Math.Max(0, 1 << (matches - 1));
            }
        }

        public int Matches 
        {
            get
            {
                return numbers.And(winners).OfType<bool>().Count(b => b);
            }
        }

        static BitArray parseNumbers(string numbers)
        {
            var ba = new BitArray(100);
            foreach (var n in numbers.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                ba.Set(int.Parse(n), true);
            }

            return ba;
        }        
    }




    private const string testInput = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

}