using System.Globalization;

namespace AdventOfCode.AoC2023;

class Day7
{

    [Solution(2023,7,1)]
    [Test(6440, testData)]
    public long Part1(Hand[] hands)
    {
        return hands
            .OrderBy(h => h)
            .Select((h,i) => (i + 1) * h.bid)
            .Sum();    
    }


    [Solution(2023,7,2)]
    [Test(5905, testData)]
    public long Part2(Hand[] hands)
    {
        return hands
            .OrderBy(h => h.comparisonWithJokers)
            .Select((h,i) => (i + 1) * h.bid)
            .Sum();    
    }


    public class Hand : IComparable<Hand>
    {
        public long[] cards;

        public long hand;

        public string comparison;

        public long bid;

        public long handWithJokers;

        public string comparisonWithJokers;

        public Hand(string input)
        {
            var parts = input.Split(" ");

            bid = long.Parse(parts[1]);
            cards = parts[0].Select(c => card(c)).ToArray();

            var distinct = cards.GroupBy(c => c);
            if (distinct.Count() == 1)
            {
                hand = 50;                
            }
            else if (distinct.Max(g => g.Count()) == 4)
            {
                hand = 40;
            }
            else if (distinct.Max(g => g.Count()) == 3)
            {
                if (distinct.Count() == 2) 
                {
                    hand = 32;
                 }
                 else
                 {
                    hand = 30;
                 } 
            }
            else if (distinct.Count(g => g.Count() == 2) == 2)
            {
                hand = 22;
            }
            else if (distinct.Count() < 5)
            {
                hand = 20;
            }
            else 
            {
                hand = 0;
            }

            comparison = hand.ToString("000") + ":" + string.Join(",", cards.Select(c => c.ToString("00")));


            handWithJokers = hand;

            var jokerCount = cards.Count(c => c == 11);
            if (jokerCount > 0) 
            {
                distinct = cards.Where(c => c != 11).GroupBy(c => c);

                if (distinct.Count() <= 1)
                {
                    handWithJokers = 50;                
                }
                else if (distinct.Max(g => g.Count()) == 3)
                {
                    handWithJokers = 30 + 10 * jokerCount;
                }
                else if (distinct.Max(g => g.Count()) == 2)
                {
                    if (distinct.Count(g => g.Count() == 2) == 2)
                    {
                        handWithJokers = 32;
                    }
                    else 
                    {
                        handWithJokers = 20 + 10 * jokerCount;
                    }                    
                }
                else 
                {
                    handWithJokers = 10 + 10 * jokerCount;
                }
            }

            comparisonWithJokers = handWithJokers.ToString("000") + ":" + string.Join(",", cards.Select(c => c.ToString("00").Replace("11","00")));

        }

        private long card(char input)
        {
            return "0123456789TJQKA".IndexOf(input);
        }

        int IComparable<Hand>.CompareTo(Hand? other)
        {
            return comparison.CompareTo(other.comparison);
        }
    }

    private const string testData = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";
}