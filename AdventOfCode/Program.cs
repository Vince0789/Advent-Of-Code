using System;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> lines = File.ReadLines("inputs.txt").ToList();
        Dictionary<int, int> elveCalories = new();

        int elve = 1;
        int calories = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                elveCalories.Add(elve, calories);
                elve++;
                calories = 0;
                continue;
            }

            calories += Convert.ToInt32(line);
        }

        // Part 1
        KeyValuePair<int, int> topElve = elveCalories.OrderByDescending(x => x.Value).FirstOrDefault();
        Console.WriteLine("The elve with the most calories is elve is carrying {0} calories.", topElve.Value);

        // Part 2
        int sumOfCalories = elveCalories.OrderByDescending(x => x.Value).Take(3).Sum(x => x.Value);
        Console.WriteLine("The top 3 elves together are carrying {0} calories in total.", sumOfCalories);

        Console.ReadLine();
    }
}