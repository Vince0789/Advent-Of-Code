using System.Text.RegularExpressions;

internal class Program
{
    struct Section
    {
        public Section(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; set; } = 0;
        public int End { get; set; } = 0;
    }

    private static void Main(string[] args)
    {
        List<string> lines = File.ReadLines("inputs.txt").ToList();
        Regex regex = new(@"^(\d+)-(\d+),(\d+)-(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        int completeOverlaps = 0;
        int overlaps = 0;

        foreach (string line in lines)
        {
            if (!ExtractInput(regex, line, out Section elve1, out Section elve2))
                continue;

            if ((elve1.Start >= elve2.Start && elve1.End <= elve2.End) || (elve2.Start >= elve1.Start && elve2.End <= elve1.End))
            {
                completeOverlaps++;
                overlaps++;
                continue; 
            }

            bool found = false;

            for (int i = elve1.Start; i <= elve1.End; i++)
            {
                if (i >= elve2.Start && i <= elve2.End)
                {
                    overlaps++;
                    found = true;
                    break;
                }
            }

            if (found) { continue; }

            for (int i = elve2.Start; i <= elve2.End; i++)
            {
                if (i >= elve1.Start && i <= elve1.End)
                {
                    overlaps++;
                    break;
                }
            }
        }

        Console.WriteLine("Part 1 - Overlaps: {0}", completeOverlaps);
        Console.WriteLine("Part 2 - Overlaps: {0}", overlaps);

        Console.ReadLine();
    }

    private static bool ExtractInput(Regex regex, string line, out Section elve1, out Section elve2)
    {
        Match match = regex.Match(line);

        if (!match.Success)
        {
            elve1 = new();
            elve2 = new();
            return false;
        }

        elve1 = new(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value));
        elve2 = new(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value));
        return true;
    }
}