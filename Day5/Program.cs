using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

internal class Program
{
    struct Instruction
    {
        public Instruction(int amount, int from, int to)
        {
            Amount= amount;
            FromStack = from - 1;
            ToStack = to - 1;
        }

        public int Amount { get; set; } = 0;
        public int FromStack { get; set; } = 0;
        public int ToStack { get; set; } = 0;
    }

    private static void Main(string[] args)
    {
        IEnumerable<string> lines = File.ReadLines("inputs.txt");

        // read lines until we reach an empty line, indicating the start of the instructions
        // reverse the lines so we truncate the stack numbers and start from the bottom of each stack
        IEnumerable<string> stacksInput = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Reverse().Skip(1);

        Stack[] stacks = new Stack[9];
        for (int i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new Stack();
        }

        foreach (string line in stacksInput)
        {
            for (int stack = 0; stack < stacks.Length; stack++)
            {
                int index = (stack * 4) + 1;
                char chr = line[index];

                if (chr >= 'A' && chr <= 'Z')
                {
                    stacks[stack].Push(chr);
                }
            }
        }

        for (int i = 0; i < stacks.Length; i++) 
        {
            Console.WriteLine("{0}: {1}", i + 1, string.Join(", ", stacks[i].ToArray()));    
        }

        // get instructions

        Regex regex = new Regex(@"^move (\d+) from (\d+) to (\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        List<Instruction> instructions = new();

        foreach (string line in lines)
        {
            Match match = regex.Match(line);
            if (!match.Success) 
                continue;

            Instruction instruction = new(
                Convert.ToInt32(match.Groups[1].Value),
                Convert.ToInt32(match.Groups[2].Value),
                Convert.ToInt32(match.Groups[3].Value)
            );

            instructions.Add(instruction);
        }

        foreach (Instruction instruction in instructions)
        {
            for (int i = 0; i < instruction.Amount; i++)
            {
                // get last element
                char? chr = (char?)stacks[instruction.FromStack].Pop();

                if (chr.HasValue)
                {
                    stacks[instruction.ToStack].Push(chr.Value);
                }
            }
        }

        for (int i = 0; i < stacks.Length; i++)
        {
            Console.WriteLine("{0}: {1}", i + 1, string.Join(", ", stacks[i].ToArray()));
        }

        Console.ReadLine();
    }
}