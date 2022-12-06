using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        string stream = File.ReadAllText("inputs.txt");

        Console.WriteLine("Part 1 - Marker: {0}", FindStartMarker(stream, 4));
        Console.WriteLine("Part 2 - Marker: {0}", FindStartMarker(stream, 14));

        Console.ReadLine();
    }

    private static int FindStartMarker(string stream, int length)
    {
        int marker = 0;
        string test;

        do
        {
            test = stream.Substring(marker, length);
            marker++;
        }
        while (HasDuplicates(test.ToArray()));

        return marker + (length - 1);
    }

    private static bool HasDuplicates(params char[] list)
    {
        List<char> seen = new();
        
        foreach (char c in list)
        {
            if (seen.Contains(c)) return true;
            seen.Add(c);
        }

        return false;
    }
}