using System.Runtime.CompilerServices;

namespace AdventOfCode.AoC2015;

class Day21
{
    [Solution(2015,21,1)]
    public int Part1(CharacterStats boss)
    {
        var lowestSpend = int.MaxValue;

        foreach (var weapon in weapons)
        {
            foreach (var armor in armor)
            {
                foreach (var ring1 in rings)
                {
                    foreach (var ring2 in rings)
                    {
                        if (ring2.Key != ring1.Key || ring2.Key == Tuple.Create(0,0))
                        {
                            var me = new CharacterStats(
                                100, 
                                weapon.Key + ring1.Key.Item1 + ring2.Key.Item1,
                                armor.Key + ring1.Key.Item2 + ring2.Key.Item2);

                            var cost = weapon.Value + armor.Value + ring1.Value + ring2.Value;

                            if (cost < lowestSpend && me.WinsIfMovingFirstAgainst(boss))
                            {
                                lowestSpend = cost;

                            }
                        }
                    }
                }
            }
        }

        return lowestSpend;

    }

    [Solution(2015,21,2)]
    public int Part2(CharacterStats boss)
    {
        var highestSpend = int.MinValue;

        foreach (var weapon in weapons)
        {
            foreach (var armor in armor)
            {
                foreach (var ring1 in rings)
                {
                    foreach (var ring2 in rings)
                    {
                        if (ring2.Key != ring1.Key || ring2.Key == Tuple.Create(0,0))
                        {
                            var me = new CharacterStats(
                                100, 
                                weapon.Key + ring1.Key.Item1 + ring2.Key.Item1,
                                armor.Key + ring1.Key.Item2 + ring2.Key.Item2);

                            var cost = weapon.Value + armor.Value + ring1.Value + ring2.Value;

                            if (cost > highestSpend && !me.WinsIfMovingFirstAgainst(boss))
                            {
                                highestSpend = cost;

                            }
                        }
                    }
                }
            }
        }

        return highestSpend;

    }    

    public class CharacterStats
    {
        public int HitPoints;
        public int Damage;
        public int Armor;

        public CharacterStats(string[] input)
        {
            HitPoints = int.Parse(input[0].Split(": ")[1]);
            Damage = int.Parse(input[1].Split(": ")[1]);
            Armor = int.Parse(input[2].Split(": ")[1]);
        }

        public CharacterStats(int HitPoints, int Damage, int Armor)
        {
            this.HitPoints = HitPoints;
            this.Damage = Damage;
            this.Armor = Armor;
        }

        public bool WinsIfMovingFirstAgainst(CharacterStats other)
        {
            var damageInflicted = Math.Max(1, Damage - other.Armor);
            var damageReceived = Math.Max(1, other.Damage - Armor);

            var turnsToKill = other.HitPoints / damageInflicted + (other.HitPoints % damageInflicted > 0 ? 1 : 0);
            var turnsToDie = HitPoints / damageReceived + (HitPoints % damageReceived > 0 ? 1 : 0);

            return turnsToKill <= turnsToDie;
        }
    }


    private Dictionary<int,int> weapons = new Dictionary<int, int> { [4] = 8, [5] = 10, [6] = 25, [7] = 40, [8] = 74 };
    private Dictionary<int,int> armor = new Dictionary<int, int> { [0] = 0, [1] = 13, [2] = 31, [3] = 53, [4] = 75, [5] = 102 };
    private Dictionary<Tuple<int,int>,int> rings = new Dictionary<Tuple<int,int>, int> { 
        [Tuple.Create(0,0)] = 0,
        [Tuple.Create(1,0)] = 25, [Tuple.Create(2,0)] = 50, [Tuple.Create(3,0)] = 100,  
        [Tuple.Create(0,1)] = 20, [Tuple.Create(0,2)] = 40, [Tuple.Create(0,3)] = 80 };

}