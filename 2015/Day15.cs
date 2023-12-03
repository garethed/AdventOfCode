using System.Runtime.InteropServices;

namespace AdventOfCode.AoC2015;

class Day15
{

    [Solution(2015,15,1)]
    public int part1() => optimise(false);

    [Solution(2015,15,2)]
    public int part2() => optimise(true);

    private int optimise(bool countCalories)
    {
        var max = int.MinValue;
        var recipe = "";

        int calculateScore(int f , int c, int b, int s)
        {
            return  Math.Max(0, ((4 * f) - b)) * 
            Math.Max(0, (-2 * f) + (5 * c)) * 
            Math.Max(0, (-1 * c) - (2 * s) + (5 * b)) * 
            (2 * s);
        }

        int calculateCalories(int f, int c, int b, int s)
        {
            return f * 5 + c * 8 + b * 6 + s * 1;
        }

        for (var f = 1; f < 100; f++)
        {
            for (var c = f / 4; c < 100 - f; c++)
            {
                for (var b = c / 5; b < 100 - f - c; b++)
                {
                    var s = 100 - f - c - b;

                    if (calculateCalories(f, c, b, s) == 500)
                    {
                        var score = calculateScore(f, c, b, s);
                        if (score > max)
                        {
                            max = score;
                            recipe = $"{f} {c} {b} {s}"; 
                        }
                    }
                }
            }
        }

        return max;
    }

    [RegexDeserializable(@"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)")]
    record Ingredient(string Name, int capacity, int durability, int flavor, int texture, int calories)
    {}

    private List<Ingredient> ingredients = RegexDeserializable.Deserialize<Ingredient>(input);

    const string input = @"Frosting: capacity 4, durability -2, flavor 0, texture 0, calories 5
Candy: capacity 0, durability 5, flavor -1, texture 0, calories 8
Butterscotch: capacity -1, durability 0, flavor 5, texture 0, calories 6
Sugar: capacity 0, durability 0, flavor -2, texture 2, calories 1";
}