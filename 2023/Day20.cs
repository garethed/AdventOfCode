using System.Data;
using System.Data.Common;
using Microsoft.VisualBasic;

namespace AdventOfCode.AoC2023;

public class Day20
{
    [Solution(2023,20,1)]
    [Test(32000000, example1)]
    [Test(11687500, example2)]
    public long Part1(string[] input)
    {
        var state = new int[input.Length];
        var modules = initModules(input, state);
        var bc = modules["roadcaster"];
        var queue = new Queue<Pulse>();
        var lowPulses = 0L;
        var highPulses = 0L;

        for (int i = 0; i < 1000; i++)
        {
            PressButton();
            if (state.Sum() == 0)
            {
                var lowPulsesPerCycle = lowPulses;
                var highPulsesPerCycle = highPulses;
                var pressesPerCycle = i + 1;

                while (i < 1000 - pressesPerCycle)
                {
                    i += pressesPerCycle;
                    lowPulses += lowPulsesPerCycle;
                    highPulses += highPulsesPerCycle;
                }
            }

        }

        return lowPulses * highPulses;


        void PressButton()
        {
            bc.Pulse(null, queue);
            lowPulses++;

            while (queue.Any())
            {
                var next = queue.Dequeue();
                if (next.high)
                {
                    highPulses += next.source.destinationModules.Count;
                }
                else
                {
                    lowPulses += next.source.destinationModules.Count;
                }

                foreach (var target in next.source.destinationModules)
                {
                    target.Pulse(next, queue);
                }
            }
        }

    }

    [Solution(2023,20,2)]
    public long Part2(string[] input)
    {
        var state = new int[input.Length];
        var modules = initModules(input, state);
        var bc = modules["roadcaster"];
        var queue = new Queue<Pulse>();
        var rx = (Output)modules["rx"];
        var firstHighOutput = new Dictionary<Module,int>();

        for (int i = 1; i < 100000; i++)
        {
            PressButton(i);
            if (rx.hasReceivedPulse) return i;
        }

        return modules["gf"].sourceModules.Aggregate(1L, (acc, mod) => acc * firstHighOutput[mod]);


        void PressButton(int i)
        {
            bc.Pulse(null, queue);

            while (queue.Any())
            {
                var next = queue.Dequeue();
                foreach (var target in next.source.destinationModules)
                {
                    target.Pulse(next, queue);
                }

                if (next.high && !firstHighOutput.ContainsKey(next.source))                
                {
                    firstHighOutput[next.source] = i;
                }
            }
        }

    }    

    private Dictionary<string,Module> initModules(string[] input, int[] state)
    {
        var modules = new Dictionary<string,Module>();
        var i = 0;

        modules["output"] = new Output() { Id = "output"};
        modules["rx"] = new Output() { Id = "rx"};

        foreach (var line in input)
        {
            var module = Module.initModule(line, i, state);
            modules[module.Id] = module;
            i++;
        }

        foreach (var line in input)
        {
            var module = modules[line.Substring(1).Split(" -> ")[0]];

            foreach (var outputId in line.Split(" -> ")[1].Split(", "))
            {
                var outputModule = modules[outputId];
                module.destinationModules.Add(outputModule);
                outputModule.sourceModules.Add(module);
            }
        }

        return modules;
    }

    abstract class Module
    {
        public string Id;
        public int Index;

        public int[] State;
        public List<Module> sourceModules = new List<Module>();
        public List<Module> destinationModules = new List<Module>();

        public abstract void Pulse(Pulse incoming, Queue<Pulse> outgoing);

        public static Module initModule(string input, int index, int[] state)
        {
            Module module;
            if (input.StartsWith("%"))
            {
                module = new FlipFlop();
            }
            else if (input.StartsWith("&"))
            {
                module = new Conjunction();
            }
            else
            {
                module = new Broadcast();
            }
            module.Id = input.Substring(1).Split(" -> ")[0];
            module.Index = index;
            module.State = state;
            return module;
        }

        public override string ToString()
        {
            return $"{Id} - {this.GetType().Name}";
        }
    }

    class Output : Module 
    {
        public bool hasReceivedPulse;

        public override void Pulse(Pulse incoming, Queue<Pulse> outgoing)
        {            
            if (!incoming.high)
            {
                hasReceivedPulse = true;
            }
        }
    }

    class FlipFlop : Module
    {
        bool isHigh
        {
            get { return State[Index] == 1; }
            set { State[Index] = value ? 1 : 0; }
        }

        public override void Pulse(Pulse incoming, Queue<Pulse> outgoing)
        {
            if (!incoming.high)
            {
                isHigh = !isHigh;
                outgoing.Enqueue(new Pulse(this, isHigh));
            }            
        }
    }

    class Conjunction : Module
    {
        int countHigh = 0;
        
        bool GetPreviousState(Module source)
        {
            return (State[Index] & 1 << (sourceModules.IndexOf(source))) > 0;
        }
        void SetPreviousState(Module source, bool value)
        {
            var mask = 1 << sourceModules.IndexOf(source);
            if (value)
            {
                State[Index] = State[Index] | mask;
            }
            else
            {
                State[Index] = State[Index] & ~mask;
            }
        }

        public override void Pulse(Pulse incoming, Queue<Pulse> outgoing)
        {
            var prev = GetPreviousState(incoming.source);
            if (incoming.high != prev)
            {
                SetPreviousState(incoming.source, incoming.high);
                countHigh += incoming.high ? 1 : -1;
            }

            outgoing.Enqueue(new Pulse(this, countHigh != sourceModules.Count));
        }
    }

    class Broadcast : Module
    {
        public override void Pulse(Pulse incoming, Queue<Pulse> outgoing)
        {
            outgoing.Enqueue(new Pulse(this, false));            
        }
    }

    record Pulse(Module source, bool high) 
    {
        public override string ToString()
        {
            return $"{source.Id} -{(high ? "high" : "low")}-> ${string.Join(',', source.destinationModules.Select(m => m.Id))}";
        }

    }


    private const string example1 = @"broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a
";

    private const string example2 = @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output
";
}