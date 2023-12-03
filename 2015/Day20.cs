namespace AdventOfCode.AoC2015;

public class Day20
{

    [Solution(2015,20,1)]
    public int LowestHouse(int input)
    {   
        var target = input / 10;   

        var houses = new int[target];

        for (int i = 1; i <= target; i++)
        {
            for (int j = i; j <= target; j += i)
            {
                houses[j - 1] += i;
            }
        }

        return houses.TakeWhile(i => i < target).Count() + 1;
    }


    [Solution(2015,20,2)]
    public int LowestHouse2(int target)
    {      
        var houses = new int[target / 10];

        for (int i = 1; i <= houses.Length; i++)
        {
            for (int j = 1; j <= 50 && j * i < houses.Length; j++)
            {
                houses[j * i - 1] += i * 11;
            }
        }

        return houses.TakeWhile(i => i < target).Count() + 1;
    }


}