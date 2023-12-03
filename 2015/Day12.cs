using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode.AoC2015;

public class Day12
{

    [Solution(2015,12,1)]
    [Test(3, @"{""a"":{""b"":4},""c"":-1}")]
    public int SumNumericValues(string inputJson)
    {
        var json = JsonDocument.Parse(inputJson);

        return SumValuesFromJson(json.RootElement, false);        
    }

    [Solution(2015,12,2)]
    [Test(4, @"[1,{""c"":""red"",""b"":2},3]")]
    public int SumExcludingRed(string inputJson)
    {
        var json = JsonDocument.Parse(inputJson);

        return SumValuesFromJson(json.RootElement, true);        
    }


    private int SumValuesFromJson(JsonElement element, bool excludeRed)
    {
        if (element.ValueKind == JsonValueKind.Number) 
        {
            return element.GetInt32();
        }
        else if (element.ValueKind == JsonValueKind.Object)
        {
            if (excludeRed && element.EnumerateObject().Any(p => p.Value.ToString() == "red")) return 0;

            return element.EnumerateObject().Sum(p => SumValuesFromJson(p.Value, excludeRed));
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            return element.EnumerateArray().Sum(e => SumValuesFromJson(e, excludeRed));
        }
        else 
        {
            return 0;
        }
    }
}