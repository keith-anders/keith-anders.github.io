using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

class Program
{
    public static async Task Main(string[] args)
    {
        await StartCalculating(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
    }

    static Task StartCalculating(string username, bool startInputThread = true)
    {
        WriteLine("Allocating a 1GB array and populating with random data. Please wait a minute...");
        byte[] bytes = new byte[1024 * 1024 * 1024];
        s_random.NextBytes(bytes);
        WriteLine("Array has been populated. Searching for most frequent entry...");
        (byte theByte, int theFrequency) = GetMostFrequentByte(bytes);

        var calculateTask = Task.Run(() =>
        {
            WriteLine("The byte {0,-3} occurred {1,-7} times in a buffer of size {2}.",
                theByte, theFrequency, bytes.Length);
        });

        if (startInputThread)
            return ConsoleInputLoop(threadId => $"Hello, {username} on thread {threadId}> ");
        return null;
    }

    static Random s_random = new Random();

    static Task ConsoleInputLoop(Func<int, string> greetingGenerator)
    {
        return Task.Run(() =>
        {
            while (true)
            {
                Write(greetingGenerator(Thread.CurrentThread.ManagedThreadId));
                string line = ReadLine();
                switch (line.ToUpper())
                {
                    case "GC":
                        GC.Collect();
                        WriteLine("Garbage collector ran successfully!!");
                        break;
                    case "":
                        WriteLine("Starting another calculation.");
                        StartCalculating(null, false);
                        break;
                    case "Q":
                        WriteLine("Good-bye!");
                        return;
                    default:
                        WriteLine("Sorry--I don't know that command.");
                        break;
                }
            }
        });
    }

    static (byte mode, int occurrences) GetMostFrequentByte(byte[] bytes)
    {
        int[] counts = new int[256];
        foreach (byte b in bytes)
            ++(counts[b]);
        byte currentWinner = 0;
        int maxScore = 0;
        for (byte index = 0; index < counts.Length; ++index)
        {
            int value = counts[index];
            if (value > maxScore)
            {
                maxScore = value;
                currentWinner = index;
            }
        }
        return (currentWinner, maxScore);
    }
}
