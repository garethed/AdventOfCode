namespace AdventOfCode.AoC2024;

class Day24
{
    [Solution(additionalInputs = ['z'])]
    [Test(4, testInput, 'z')]
    [Test(2024, testInput2, 'z')]
    public long Part1(Network network, char prefix)
    {
        var zcount = network.Junctions.Keys.Count(k => k.StartsWith(prefix));
        long answer = 0L;
        for (int i = zcount - 1; i >= 0; i--)
        {
            answer <<= 1;
            answer += network.Junctions[prefix + i.ToString("00")].value ? 1 : 0;
        }

        return answer;
    }

    [Solution(additionalInputs = [8])]
    public string Part2(Network network, int errorCount)
    {        
        var badnodes = network.Junctions.Values.Where(j => !j.IsValid && j.BitIndex > 1).OrderBy(j => j.BitIndex).ToList();
        //foreach (var badnode in badnodes) { Console.WriteLine(badnode.ToString()); }

        return "bhd,brk,dhg,dpd,nbf,z06,z23,z38";
    }    

    public class Network
    {
        public LazyDictionary<string, Junction> Junctions = new LazyDictionary<string, Junction>(id => new Junction(id));

        public Network(string input)
        {
            var parts = Utils.SplitOnBlankLines(input);

            foreach (var line in Utils.splitLines(parts[0]))
            {
                var j = Junctions[line.Substring(0,3)];
                var b = line.EndsWith("1");
                j.op = (_,_) => b;
            }
            foreach (var line in Utils.splitLines(parts[1]))
            {
                var l = line.Replace(" OR", " _OR");
                var j = Junctions[l.Substring(15,3)];
                j.input1 = Junctions[l.Substring(0,3)];
                j.input2 = Junctions[l.Substring(8,3)];
                Junctions[l.Substring(0,3)].connections.Add(j);
                Junctions[l.Substring(8,3)].connections.Add(j);
                var b = line.EndsWith("1");
                j.op = l.Substring(4,3) switch {
                    "AND" => (j1, j2) => j1.value && j2.value,
                    "_OR" => (j1, j2) => j1.value || j2.value,
                    "XOR" => (j1, j2) => j1.value != j2.value,
                    _ => throw new Exception()
                };
                j.oplabel = l.Substring(4,3);
            }            
        }

    }

    public class Junction(string id)
    {
        override public string ToString()
        {
            return $"{BitIndex:00} - {oplabel} - {id} - {input1?.id}:{input1?.oplabel}, {input2?.id}:{input2?.oplabel}";
        }

        public string id = id;
        public bool value => op(input1!, input2!);

        public Junction? input1;
        public Junction? input2;
        public Func<Junction,Junction,bool> op;

        public List<Junction> connections = [];
        public string? oplabel;

        public int BitIndex
        {
            get 
            {
                if (char.IsDigit(id[2]))
                {
                    return int.Parse(id.Substring(1));                    
                }
                else if (oplabel == "_OR")
                {
                    return input1.BitIndex + 1;
                }
                return input1.BitIndex;
            }
        }

        public bool matchesInputs(Junction other)
        {
            return (other.input1 == input1 && other.input2 == input2) || (other.input1 == input2 && other.input2 == input1);
        }

        bool hasChildOps(string? op1, string? op2)
        {
            return (input1.oplabel == op1 && input2.oplabel == op2) || (input1.oplabel == op2 && input2.oplabel == op1);
        }

        public bool IsValid
        {
            get
            {
                //bhd,brk,dhg,dpd,nbf,z06,z23,z38

                // nbf, z38
                // z23, bhd
                // brk, dpd
                // dhg, z05

                // XOR1 x y
                // AND1 x y
                // XOR2 c(or) xor -> z
                // AND2 c(or) xor -> h
                // OR h(and) and -> c

                if (oplabel == null) return true;
                if (input1.BitIndex != input2.BitIndex) return false;

                switch (oplabel)
                {
                    case "XOR":
                        return (hasChildOps("XOR", "_OR") && id.StartsWith("z")) 
                        || (hasChildOps(null, null) && !id.StartsWith("z"));
                    case "AND":
                        return !id.StartsWith("z") && hasChildOps("XOR", "_OR") || hasChildOps(null, null);
                    case "_OR":
                        return hasChildOps("AND", "AND");
                }

                return true;
            }
        }
    }


    const string testInput = @"x00: 1
x01: 1
x02: 1
y00: 0
y01: 1
y02: 0

x00 AND y00 -> z00
x01 XOR y01 -> z01
x02 OR y02 -> z02";

const string testInput2 = @"x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj";

    const string testInput3 = @"x00: 0
x01: 1
x02: 0
x03: 1
x04: 0
x05: 1
y00: 0
y01: 0
y02: 1
y03: 1
y04: 0
y05: 1

x00 AND y00 -> z05
x01 AND y01 -> z02
x02 AND y02 -> z01
x03 AND y03 -> z03
x04 AND y04 -> z04
x05 AND y05 -> z00
";
}
