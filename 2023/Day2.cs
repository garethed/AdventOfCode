namespace AdventOfCode.AoC2023;

class Day2
{

    [Solution(2023,2,1)]
    [Test(8, testData)]
    public int Part1(Game[] games)
    {        
        return games.Where(g => g.MaxRed() <= 12 && g.MaxGreen() <= 13 && g.MaxBlue() <= 14).Sum(g => g.Id);
    }

    [Solution(2023,2,2)]
    [Test(2286, testData)]
    public int Part2(Game[] games)
    {
        return games.Sum(g => g.MaxGreen() * g.MaxBlue() * g.MaxRed());
    }

    public class Game
    {
        public int Id;
        public List<Turn> Turns = new();

        public Game(string line)
        {
            var parts = line.Split(":");
            Id = int.Parse(parts[0].Split(" ")[1]);

            Turns = parts[1].Split(";").Select(s => new Turn(s)).ToList();
        }        

        public int MaxRed() 
        {
            return Turns.Max(t => t.red);
        }

        public int MaxBlue() 
        {
            return Turns.Max(t => t.blue);
        }

        public int MaxGreen() 
        {
            return Turns.Max(t => t.green);
        }
    }

    public class Turn
    {
        public int red;
        public int blue;
        public int green;

        public Turn(string s)
        {
            foreach (var part in s.Split(","))
            {
                var p2 = part.Split(" ");
                var count = int.Parse(p2[1]);
                
                switch (p2[2])
                {
                    case "red": 
                        red = count; 
                        break;
                    case "blue": 
                        blue = count;
                        break;
                    case "green":
                        green = count;
                        break;                        
                }                
            }
        }
    }

    private const string testData = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";
}
