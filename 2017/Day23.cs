namespace AdventOfCode.AoC2017;

public class Day23
{
    [Solution]
    public long Part1(Program p)
    {
        p.Run();
        return p.mc;

    }

    [Solution]
    public long Part2(Program p)
    {
        var c = 0;
        for (int b = 107900; b <= 124900; b += 17)
        {
            if (!isPrime(b))
            {
                c++;
            }
        }
        
        return c;
    }    


    bool isPrime(int i)
    {
        for (int j = 2; j <= i / 2; j++)
        {
            if (i % j == 0)
            {
            return false;
            }
        }
        return true;
    }

/*

        set b 79       -> b init to 79
        set c b        -> c init to 79
        jnz a 2        -> skip multipliers if debug
        jnz 1 5
        mul b 100      -> b = 7900
        sub b -100000  -> b 107900
        set c b        -> c = b
        sub c -17000   -> c 124900

f1      set f 1
        set d 2
e2      set e 2
gd          set g d     -> set g = d
            mul g e     -> mul g by e
            sub g b     -> sub b from g
            jnz g 2     -> set f = 0 if g = 0 i.e. if d * e = b
            set f 0
            sub e -1    -> add 1 to e
            set g e     -> set g to e
            sub g b     -> sub b from g
            jnz g -8    -> loop gd unless g = 0 i.e while e < b
        sub d -1
        set g d
        sub g b
        jnz g -13   -> e2 loop while d < b

        jnz f 2     -> increase h if f was 0
        sub h -1

        set g b     -> set g = b so exit if b == c
        sub g c     -> sub c from g
        jnz g 2     -> exit if g = 0
        jnz 1 3     -> exit
        sub b -17   -> only place that changes b
        jnz 1 -23   -> f1



*/





    public class Program
    {
        public int[] Registers = new int[8];
        int pc = 0;
        public int mc = 0;
        Action[] commands;

        public void Run()
        {
            pc = 0;
            mc = 0;

            while (pc >= 0 && pc < commands.Length)
            {
                commands[pc]();
                pc++;
            }
        }
        
        public Program(string[] input)
        {
            commands = input.Select(i => parseCommand(i)).ToArray();

            Func<int> get(string s)
            {
                var register = "abcdefgh".IndexOf(s);
                if (register >= 0)
                {
                    return () => Registers[register];
                }
                else
                {
                    var value = int.Parse(s);
                    return () => value;
                }
            }

            Action<int> set(string s)
            {
                var register = "abcdefgh".IndexOf(s);
                return i => Registers[register] = i;
            }

            Action parseCommand(string command)
            {
                var parts = command.Split(' ');
                var g1 = get(parts[1]);
                var g2 = get(parts[2]);
                var s1 = set(parts[1]);

                switch (parts[0])
                {
                    case "jnz":
                        return () => { if (g1() != 0) { pc += g2() - 1; } };
                    case "set":
                        return () => s1(g2());
                    case "sub":
                        return () => s1(g1() - g2());
                    case "mul":
                        return () => { mc++; s1(g1() * g2()); };
                    default:
                        throw new Exception();
                }        
            }
        }

    }
}
