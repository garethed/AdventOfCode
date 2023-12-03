using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace AdventOfCode.AoC2015;

public class Day6
{

    [Solution(2015,6,1)]
    [Test(4, "turn on 499,499 through 500,500")]
    [Test(90, "turn on 0,0 through 9,9\ntoggle 0,0 through 0,9")]
    [Test(8, "turn on 0,0 through 3,3\ntoggle 0,0 through 1,3\ntoggle 0,0 through 3,1")]
    public int ApplyAllInstructions(string input)
    {
       
        var instructions = RegexDeserializable.Deserialize<Instruction>(input);
        instructions.Reverse();

        var w = instructions.Max(i => i.xmax) + 1;
        var h = instructions.Max(i => i.ymax) + 1;

        var lights = new LightState[w, h];

        var count = Point2.enumerateGrid(w, h).Select(p => ApplyInstructions(instructions, p.x, p.y, lights)).Count(i => i == LightState.On);        
        return count;
    }

    private LightState ApplyInstructions(IEnumerable<Instruction> instructionsReversed, int x, int y, LightState[,] lights)
    {
        var state = LightState.Unknown;

        foreach (var instruction in instructionsReversed) 
        {
            state = ApplyInstruction(instruction, x, y, state);
            if (state == LightState.On || state == LightState.Off) 
            {
                lights[x,y] = state;
                return state;
            }            
        }

        state = state == LightState.Unknown ? LightState.Off : LightState.On;
        lights[x,y] = state;
        return state;

    }

    private LightState ApplyInstruction(Instruction instruction, int x, int y, LightState previousState)
    {
        if (!instruction.AppliesTo(x,y)) return previousState;

        switch (instruction.operation)
        {
            case "turn on":                
                return previousState == LightState.Inverted ? LightState.Off : LightState.On;
            case "turn off":
                return previousState == LightState.Inverted ? LightState.On : LightState.Off;
            case "toggle":            
                return previousState == LightState.Inverted ? LightState.Unknown : LightState.Inverted;
            default:
                throw new InvalidOperationException();

        }
    }

    [Solution(2015,6,2)]
    public int ApplyAllInstructions2(string input)
    {
        var instructions = RegexDeserializable.Deserialize<Instruction>(input);

        var w = instructions.Max(i => i.xmax) + 1;
        var h = instructions.Max(i => i.ymax) + 1;

        var brightnesses = new int[w, h];

        var count = Point2.enumerateGrid(w, h).Select(p => ApplyInstructions2(instructions, p.x, p.y, brightnesses)).Sum(); 
        return count;
    }    

    private int ApplyInstructions2(IEnumerable<Instruction> instructions, int x, int y, int[,] brightnesses)
    {
        var brightness = 0;

        foreach (var instruction in instructions) 
        {
            brightness = ApplyInstruction2(instruction, x, y, brightness);
        }

        brightnesses[x,y] = brightness;
        return brightness;
    }    

    private int ApplyInstruction2(Instruction instruction, int x, int y, int previousBrightness)
    {
        if (!instruction.AppliesTo(x,y)) return previousBrightness;

        switch (instruction.operation)
        {
            case "turn on":                
                return previousBrightness + 1;
            case "turn off":
                return Math.Max(previousBrightness - 1, 0);
            case "toggle":            
                return previousBrightness + 2;
            default:
                throw new InvalidOperationException();

        }
    }    

    [RegexDeserializable(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)")]
    record Instruction(string operation, int xmin, int ymin, int xmax, int ymax) {
        public bool AppliesTo(int x, int y) => x >= xmin && x <= xmax && y >= ymin && y <= ymax;

    }

    
    enum LightState
    {
        Unknown,
        Inverted,
        On,
        Off
    }

}