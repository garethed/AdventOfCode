namespace AdventOfCode.AoC2023;

public class Day19
{
    [Solution(2023,19,1)]
    [Test(19114, testData)]
    public long Part1(string input)
    {
        var sections = input.Split("\n\n").Select(s => Utils.splitLines(s)).ToArray();
        var workflows = initWorkflows(sections[0]);
        var data = sections[1].Select(l => new Xmas(l.Split("{xmas=,}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray()));
        var start = workflows["in"];

        return data.Where(d => shouldAccept(d)).Sum(data => data.Rating);


        bool shouldAccept(Xmas xmas)
        {
            var wf = start;
            while (!wf.Final)
            {
                wf = wf.execute(xmas);
            }

            return wf == Workflow.Accept;
        }
    }

    [Solution(2023,19,2)]
    [Test(167409079868000L, testData)]
    public long Part2(string input)
    {
        var sections = input.Split("\n\n").Select(s => Utils.splitLines(s)).ToArray();
        var workflows = initWorkflows(sections[0]);
        var range = new XmasRange(new[] { 1,1,1,1}, new[] { 4000, 4000, 4000, 4000 }, workflows["in"]);
        var queue = new Queue<XmasRange>();
        queue.Enqueue(range);

        var count = 0L;
        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            count += next.nextWorkflow.executeRange(next, queue);
        }

        return count;
        
    }

    Dictionary<string,Workflow> initWorkflows(string[] input)
    {
        var workflows = new Dictionary<string,Workflow>();
        workflows["A"] = Workflow.Accept;
        workflows["R"] = Workflow.Reject;

        foreach (var wfline in input)
        {
            initWorkflow(wfline);
        }

        return workflows;
        
        void initWorkflow(string description)
        {
            var parts = description.Split("{,}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var wf = getWorkflow(parts[0]);
            wf.defaultStep = getWorkflow(parts.Last());
            
            foreach (var step in parts.Skip(1).Take(parts.Length - 2))
            {                
                var stepParts = step.Split(':');
                var i = "xmas".IndexOf(step[0]);
                var n = int.Parse(stepParts[0].Substring(2));
                var nextWf = getWorkflow(stepParts[1]);
                if (step[1] == '>') 
                {
                    wf.splits.Add((xmas, q) => xmas.splitGreaterThan(i, n, nextWf, q));
                    wf.conditions.Add(xmas => xmas.data[i] > n);
                }
                else
                {
                    wf.splits.Add((xmas, q) => xmas.splitLessThan(i, n, nextWf, q));
                    wf.conditions.Add(xmas => xmas.data[i] < n);
                }
                wf.nextSteps.Add(nextWf);
            }
                    
        }

        Workflow getWorkflow(string Id) 
        {
            if (!workflows.ContainsKey(Id))
            {
                workflows[Id] =new Workflow(Id);
            }

            return workflows[Id];
        }        
    }

    record Xmas(int[] data) {
        public int Rating => data.Sum();
    }

    record XmasRange(int[] min, int[] max, Workflow nextWorkflow) {

        public long Size => (long)(max[0] - min[0] + 1) * (long)(max[1] - min[1] + 1) * (long)(max[2] - min[2] + 1) * (long)(max[3] - min[3] + 1);

        public void splitLessThan(int i, int n, Workflow next, Queue<XmasRange> processingQueue)
        {
            if (n > min[i] && n <= max[i])
            {
                var newMin = (int[])min.Clone();
                var newMax = (int[])max.Clone();

                newMax[i] = n - 1;
                var newRange = new XmasRange(newMin, newMax, next);
                processingQueue.Enqueue(newRange);

                min[i] = n;
            }            
        }

        public void splitGreaterThan(int i, int n, Workflow next, Queue<XmasRange> processingQueue)
        {
            if (n >= min[i] && n < max[i])
            {
                var newMin = (int[])min.Clone();
                var newMax = (int[])max.Clone();

                newMin[i] = n + 1;
                var newRange = new XmasRange(newMin, newMax, next);
                processingQueue.Enqueue(newRange);

                max[i] = n;
            }            
        }        

    }    


    class Workflow
    {
        public string Id;
        public List<Func<Xmas,bool>> conditions = new List<Func<Xmas, bool>>();
        public List<Action<XmasRange, Queue<XmasRange>>> splits = new List<Action<XmasRange, Queue<XmasRange>>>();
        public List<Workflow> nextSteps = new List<Workflow>();

        public Workflow defaultStep;

        public Workflow execute(Xmas xmas)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i](xmas)) return nextSteps[i];
            }

            return defaultStep;
        }

        public long executeRange(XmasRange range, Queue<XmasRange> processingQueue)
        {
            if (this == Accept)
            {
                return range.Size;
            }
            else if (this == Reject)
            {
                return 0;
            }
            else 
            {
                foreach (var split in splits)
                {
                    split(range, processingQueue);
                }
                return defaultStep.executeRange(range, processingQueue);
            }
        }

        public static Workflow Reject = new Workflow("R");
        public static Workflow Accept = new Workflow("A");

        public Workflow(string Id ) { this.Id = Id; }

        public bool Final => char.IsUpper(Id[0]);
        
    }

    private const string testData = @"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";
}