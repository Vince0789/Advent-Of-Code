using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    struct Instruction
    {
        public Instruction(int amount, int from, int to)
        {
            // decrease stack numbers by one because array indexes start at 0
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
        // then reverse the lines, truncate the stack numbers, and start from the bottom of each stack
        IEnumerable<string> stacksInput = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Reverse().Skip(1);

        // defining a separate stack for each part, because cloning is bullshit
        Stack[] part1Stacks = new Stack[9];
        Stack[] part2Stacks = new Stack[9];

        for (int i = 0; i < part1Stacks.Length; i++)
        {
            part1Stacks[i] = new Stack();
            part2Stacks[i] = new Stack();
        }

        // read lines, starting from the bottom of each stack and pushing them onto their respective stacks
        foreach (string line in stacksInput)
        {
            for (int stack = 0; stack < part1Stacks.Length; stack++)
            {
                int index = (stack * 4) + 1;
                char chr = line[index];

                if (chr >= 'A' && chr <= 'Z')
                {
                    part1Stacks[stack].Push(chr);
                    part2Stacks[stack].Push(chr);
                }
            }
        }

        // get instructions
        Regex regex = new(@"^move (\d+) from (\d+) to (\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

        // execute the instructions

        foreach (Instruction instruction in instructions)
        {
            // Part 1 - Just pop however many from the first stack and push them onto the other stack
            for (int i = 0; i < instruction.Amount; i++)
            {
                // get last element
                char? chr = (char?)part1Stacks[instruction.FromStack].Pop();

                if (chr.HasValue)
                {
                    part1Stacks[instruction.ToStack].Push(chr.Value);
                }
            }

            // Part 2 - Pop the necessary number off of one stack and store in a buffer,
            // then push the buffer onto the target stack in reverse order.
            List<char> buffer = new();

            for (int i = 0; i < instruction.Amount; i++)
            {
                // get last element
                char? chr = (char?)part2Stacks[instruction.FromStack].Pop();

                if (chr.HasValue)
                {
                    buffer.Add(chr.Value);
                }
            }

            buffer.Reverse();
            foreach (char chr in buffer)
            {
                part2Stacks[instruction.ToStack].Push(chr);
            }
        }

        // output results
        StringBuilder sb = new();
        sb.Append("Part 1: ");
        
        for (int i = 0; i < part1Stacks.Length; i++)
        {
            char? top = (char?)part1Stacks[i].Peek();
            if (top.HasValue)
            {
                sb.Append(top.Value);
            }
        }

        sb.AppendLine();
        sb.Append("Part 2: ");

        for (int i = 0; i < part2Stacks.Length; i++)
        {
            char? top = (char?)part2Stacks[i].Peek();
            if (top.HasValue)
            {
                sb.Append(top.Value);
            }
        }

        sb.AppendLine();
        Console.WriteLine("{0}", sb.ToString());

        Console.ReadLine();
    }
}
