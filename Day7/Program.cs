using System.Reflection.Emit;
using System.Text.RegularExpressions;

internal class Program
{
    public class File
    {
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; } = default;
    }

    public class Directory
    {
        public string Name { get; set; } = string.Empty;
        public Directory? Parent = null;
        public List<Directory> SubDirectories { get; set; } = new();
        public List<File> Files { get; set; } = new();
    }

    private static void Main(string[] args)
    {
        IEnumerable<string> lines = System.IO.File.ReadLines("inputs.txt");
        LinkedList<string> currentPath = new();

        Directory currentDirectory = new();

        foreach (string line in lines.Skip(1)) // skip cd to root
        {
            // it's a command
            if (TryGetCommand(line, out string command, out string arg))
            {
                if (command == "cd")
                {
                    if (arg == "..") // move up one dir
                    {
                        if (currentDirectory.Parent != null)
                        {
                            currentDirectory = currentDirectory.Parent;
                        }
                    }
                    else
                    {
                        currentDirectory = currentDirectory.SubDirectories.First(x => x.Name == arg);
                    }
                }
                else if (command == "ls")
                {

                }
            }
            else // it's a directory listing
            {
                Regex regex = new(@"^(\d+|dir)\s+(.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match match = regex.Match(line);

                if (match.Success)
                {
                    if (match.Groups[1].Value == "dir") // it's a subdirectory
                    {
                        string directoryName = match.Groups[2].Value;
                        if (!currentDirectory.SubDirectories.Any(x => x.Name == directoryName))
                        {
                            currentDirectory.SubDirectories.Add(new Directory() { 
                                Name = directoryName,
                                Parent = currentDirectory,
                            });
                        }
                    }
                    else // it's a file
                    {
                        int size = Convert.ToInt32(match.Groups[1].Value);
                        string fileName = match.Groups[2].Value;

                        if (!currentDirectory.Files.Any(x => x.Name == fileName))
                        {
                            currentDirectory.Files.Add(new File() { Name = fileName, Size = size });
                        }
                    }
                }
            }
        }

        // traverse back up the tree
        while (currentDirectory.Parent != null)
        {
            currentDirectory = currentDirectory.Parent;
        }

        int sum = 0;
        //PrintDirectoryContents(currentDirectory, includeFiles: false, level: 0, ref sum);
        GetDirectorySize(currentDirectory, ref sum, 100000);

        Console.WriteLine("Part 1: {0}", sum);

        Console.WriteLine("Part 2:");
        const int diskSpace = 70000000;
        const int requiredSpace = 30000000;

        int totalSpaceUsed = GetTotalDirectorySize(currentDirectory);
        Console.WriteLine("total space used: {0}", totalSpaceUsed);

        int freeSpace = diskSpace - totalSpaceUsed;
        Console.WriteLine("free space: {0}", freeSpace);

        int spaceToFree = requiredSpace - freeSpace;
        Console.WriteLine("space to free: {0}", spaceToFree);

        Dictionary<Directory, int> all = new();
        GetAllDirectoriesWithSize(currentDirectory, ref all);

        var candidate = all.Where(x => x.Value >= spaceToFree).OrderBy(x => x.Value).First();
        Console.WriteLine("smallest directory to delete: {0}, size {1}", candidate.Key.Name, candidate.Value);

        Console.ReadLine();
    }

    private static void GetAllDirectoriesWithSize(Directory directory, ref Dictionary<Directory, int> list)
    {
        list.Add(directory, GetTotalDirectorySize(directory));

        foreach (var subdir in directory.SubDirectories)
        {
            GetAllDirectoriesWithSize(subdir, ref list);
        }
    }

    private static void GetDirectorySize(Directory directory, ref int sum, int? limit = null)
    {
        int size = GetTotalDirectorySize(directory);

        if (limit == null || size <= limit)
        {
            sum += size;
        }

        foreach (var subdir in directory.SubDirectories)
        {
            GetDirectorySize(subdir, ref sum, limit);
        }
    }

    private static void PrintDirectoryContents(Directory directory, bool includeFiles, int level, ref int sum)
    {
        string indent = new(' ', level * 2);
        int size = GetTotalDirectorySize(directory);
        Console.WriteLine("{0}{1}: {2}", indent, directory.Name, size);

        if (size <= 100000)
        {
            sum += size;
        }

        foreach (var subdir in directory.SubDirectories)
        {
            PrintDirectoryContents(subdir, includeFiles, level + 1, ref sum);
        }

        if (includeFiles)
        {
            foreach (var file in directory.Files)
            {
                Console.WriteLine("{0}{1}: {2}", indent, file.Name, file.Size);
            }
        }
    }

    private static int GetTotalDirectorySize(Directory directory)
    {
        int total = directory.Files.Sum(x => x.Size);

        foreach (var subdir in directory.SubDirectories)
        {
            total += GetTotalDirectorySize(subdir);
        }

        return total;
    }

    private static bool TryGetCommand(string input, out string command, out string arg)
    {
        Regex regex = new(@"^\$\s+(\S+)(?:\s+(.+))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Match match = regex.Match(input);

        if (!match.Success)
        {
            command = string.Empty;
            arg = string.Empty;
            return false;
        }

        command = match.Groups[1].Value;
        arg = match.Groups[2].Value;
        return true;
    }
}