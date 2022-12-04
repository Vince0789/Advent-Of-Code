using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> lines = File.ReadLines("inputs.txt").ToList();

        #region Part 1
        int sumOfPriorities = 0;

        foreach (string line in lines)
        {
            int len = line.Length;
            string compartment1 = line[..(len / 2)];
            string compartment2 = line[(len / 2)..];

            foreach (char item in compartment1)
            {
                if (compartment2.IndexOf(item) != -1)
                {
                    sumOfPriorities += GetPriority(item);
                    break;
                }
            }
        }

        Console.WriteLine("Part 1 - The sum of the priorities is {0}.", sumOfPriorities);
        #endregion

        #region Part 2
        sumOfPriorities = 0;
        const int numElvesInGroup = 3;

        for (int i = 0; i < lines.Count; i += numElvesInGroup)
        {
            List<string> groupRucksacks = new();

            for (int j = 0; j < numElvesInGroup; j++)
            {
                groupRucksacks.Add(lines[i + j]);
            }

            foreach (char item in groupRucksacks.First())
            {
                if (groupRucksacks.All(x => x.IndexOf(item) != -1))
                {
                    sumOfPriorities += GetPriority(item);
                    break;
                }
            }
        }

        Console.WriteLine("Part 2 - The sum of the priorities is {0}.", sumOfPriorities);
        #endregion
        
        Console.ReadLine();
    }

    private static int GetPriority(char item)
    {
        if (item >= 'a' && item <= 'z')
            return (item - 'a') + 1;

        if (item >= 'A' && item <= 'Z')
            return 26 + (item - 'A') + 1;

        return 0;
    }
}