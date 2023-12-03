namespace AdventOfCode.AoC2015;

public class Day13
{

    [Solution(2015,13,1)]
    [Test(330, testData)]
    public int MaximiseHappiness(List<Pairing> pairings)
    {
        var scores = pairings.ToDictionary(p => p.guest1 + p.guest2, p => (p.gainOrLose == "gain" ? 1 : -1) * p.units);        
        var guests = pairings.Select(p => p.guest1).Distinct().ToHashSet();
        var count = guests.Count;
        var seats = new string?[count];
        seats[0] = guests.First();
        guests.Remove(seats[0]);

        var maxHappiness = int.MinValue;
        string[] maxHappinessSeats = new string[0];

        seatGuests(1, 0);

        return maxHappiness;

        int happinessDelta(int seat1, int seat2)
        {
            if (seats[seat1] == "me" || seats[seat2] == "me") return 0;
            return scores[seats[seat1] + seats[seat2]] + scores[seats[seat2] + seats[seat1]];
        }

        void seatGuests(int nextSeat, int currentHappiness)
        {
            if (nextSeat == count)
            {
                currentHappiness += happinessDelta(0, count - 1) ;
                if (currentHappiness > maxHappiness)
                {
                    maxHappiness = currentHappiness;
                    maxHappinessSeats = (string[])seats.Clone();
                }
                return;
            }

            foreach (var guest in guests)
            {
                if (Array.IndexOf(seats, guest) < 0)
                {
                    seats[nextSeat] = guest;
                    var newHappiness = currentHappiness + happinessDelta(nextSeat - 1, nextSeat);                    
                    seatGuests(nextSeat + 1, newHappiness);
                    seats[nextSeat] = null;                
                }
            }
        }
    }

    [Solution(2015,13,2)]
    public int MaximiseHappinessIncludingMe(List<Pairing> pairings)
    {
        pairings.Add(new Pairing("me", "gain", 0, "me"));
        return MaximiseHappiness(pairings);
    }

    //Eric would gain 34 happiness units by sitting next to George.
    [RegexDeserializable(@"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+).")]
    public record Pairing(string guest1, string gainOrLose, int units, string guest2) { }


    const string testData = @"Alice would gain 54 happiness units by sitting next to Bob.
Alice would lose 79 happiness units by sitting next to Carol.
Alice would lose 2 happiness units by sitting next to David.
Bob would gain 83 happiness units by sitting next to Alice.
Bob would lose 7 happiness units by sitting next to Carol.
Bob would lose 63 happiness units by sitting next to David.
Carol would lose 62 happiness units by sitting next to Alice.
Carol would gain 60 happiness units by sitting next to Bob.
Carol would gain 55 happiness units by sitting next to David.
David would gain 46 happiness units by sitting next to Alice.
David would lose 7 happiness units by sitting next to Bob.
David would gain 41 happiness units by sitting next to Carol.";

}