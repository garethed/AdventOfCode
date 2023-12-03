using System.Data;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace AdventOfCode.AoC2015;

public class Day7
{

    [Solution(2015,7,1)]
    [Test(65079, testData)]
    public int Part1(string[] input)
    {
        var signals = BuildCircuit(input);
        return (int)signals["a"].value;
    }

    [Solution(2015,7,2)]
    public int Part2(string[] input)
    {
        var signals = BuildCircuit(input);
        var a =signals["a"].value;
        signals["b"].SetValue((ushort)a);
        return (int)signals["a"].value;
    }





    private Dictionary<string,Signal> BuildCircuit(string[] input)
    {
        var signals = new Dictionary<string, Signal>();
        Signal getSignal(string id) => int.TryParse(id, out _) ? new Signal() {value = ushort.Parse(id)} : Utils.GetOrConstruct(signals, id);  

        foreach (var s in input)
        {
            var parts = s.Split(' ');
            var output = getSignal(parts.Last());

            switch (parts.Length)
            {
                case 3:
                    new Gate(operations["OR"], getSignal(parts[0]), getSignal("0"), output);
                    break;
                case 4:                
                    // NOT x -> h
                    new Gate(operations[parts[0]], getSignal(parts[1]), null, output);                    
                    break;                
                case 5:
                    // y RSHIFT 2 -> g
                    new Gate(operations[parts[1]], getSignal(parts[0]), getSignal(parts[2]), output);                    
                    break;

            }
        }

        return signals;
    }


    private class Signal
    {
        public ushort? value;
        private List<Gate> outputs = new();

        public void SetValue(ushort value) 
        {
            if (this.value != value)
            {
                this.value = value;            
                outputs.ForEach(g => g.Update());
            }
        }

        public void AddOutput(Gate gate)
        {
            outputs.Add(gate);
        }

        public override string ToString()
        {
            return $"{value}";
        }
    }

    private class Gate 
    {

        private Signal? s1;
        private Signal? s2;
        private Operation operation;
        private Signal output;

        public Gate(Operation operation, Signal input1, Signal? input2, Signal output)
        {
            this.operation = operation;
            this.output = output;
            this.s1 = input1;
            this.s2 = input2;
            s1.AddOutput(this);
            if (s2 != null) s2.AddOutput(this);
            Update();
        }

        public void Update() 
        {            
            if (operation.inputCount == 1 && s1?.value != null)
            {
                output.SetValue(operation.action(s1.value??0,0));
            }   
            else if (s1?.value != null && s2?.value != null)
            {
                output.SetValue(operation.action(s1.value??0, s2.value??0));
            }
        }

    }

    private record Operation(Func<ushort,ushort,ushort> action, int inputCount) {}

    Dictionary<string, Operation> operations = new()   
    {
        ["AND"] = new Operation((x,y) => (ushort)(x & y), 2),
        ["OR"] = new Operation((x,y) => (ushort)(x | y), 2),
        ["LSHIFT"] = new Operation((x,y) => (ushort)(x << y), 2),
        ["RSHIFT"] = new Operation((x,y) => (ushort)(x >> y), 2),
        ["NOT"] = new Operation((x,y) => (ushort)(~x), 1)
    };
const string testData = @"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> a
";
    
}