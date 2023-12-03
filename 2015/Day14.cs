
namespace AdventOfCode.AoC2015;

public class Day14 
{

    [Solution(2015,14,1,2503)]
    [Test(1120, testData, 1000)]
    public int leaderAfter(List<Reindeer> reindeer, int seconds)
    {
        return reindeer.Max( r => r.PositionAt(seconds));
    }

    [Solution(2015,14,2,2503)]
    [Test(689, testData, 1000)]
    public int scoreEachSecond(List<Reindeer> reindeer, int seconds)
    {
        for (int i = 1; i <= seconds; i++)
        {
            var max = leaderAfter(reindeer, i);
            foreach (var r in reindeer)
            {
                if (r.PositionAt(i) == max)
                {
                    r.score++;
                }
            }
        }

        return reindeer.Max(r => r.score);

    }






    // Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
    [RegexDeserializable(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds")]
    public record Reindeer(string name, int flyingSpeed, int flyingDuration, int restingDuration)
    {
        public int score;

        public int PositionAt(int seconds)
        {
            var cycles = seconds / (flyingDuration + restingDuration);
            var remainder = seconds % (flyingDuration + restingDuration);

            return cycles * (flyingSpeed * flyingDuration) + flyingSpeed * Math.Min(remainder, flyingDuration);        
        }
    }

    const string testData = @"
    Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
    Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.";
}