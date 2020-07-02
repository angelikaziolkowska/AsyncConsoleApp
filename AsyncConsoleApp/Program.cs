using AsyncConsoleApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static Tot tot = new Tot();

    public Program(int total)
    {       
        tot.Result = total;
    }

    static async Task Main()
    {
        //int num = 0;
        //while (true)
        //{
        //    // Start computation.
        //    Example(num);

        //    // Handle user input.
        //    num = Int32.Parse(Console.ReadLine());            
        //    Console.WriteLine($"You typed: {num}.");           
        //}

        await RunTeleprompter();
    }

    static IEnumerable<string> ReadFrom(string file)
    {
        string line;
        using (var reader = File.OpenText(file))
        {
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(' ');
                var lineLength = 0;
                foreach (var word in words)
                {
                    yield return word + " ";
                    lineLength += word.Length + 1;
                    if (lineLength > 70)
                    {
                        yield return Environment.NewLine;
                        lineLength = 0;
                    }
                }
                yield return Environment.NewLine;
            }
        }
    }

    private static async Task RunTeleprompter()
    {
        var config = new TelePrompterConfig();
        var displayTask = ShowTeleprompter(config);
        var speedTask = GetInput(config);
        await Task.WhenAny(displayTask, speedTask);
    }

    private static async Task ShowTeleprompter(TelePrompterConfig config)
    {
        var words = ReadFrom("sampleQuotes.txt");
        foreach (var word in words)
        {
            Console.Write(word);
            if (!string.IsNullOrWhiteSpace(word))
            {
                await Task.Delay(config.DelayInMilliseconds);
            }
        }
        config.SetDone();
    }

    private static async Task GetInput(TelePrompterConfig config)
    {
       Action work = () =>
       {
           do
           {
               var key = Console.ReadKey(true);
               if (key.KeyChar == '>')
                   config.UpdateDelay(-10);
               else if (key.KeyChar == '<')
                   config.UpdateDelay(10);
               else if (key.KeyChar == 'X' || key.KeyChar == 'x')
                   config.SetDone();
           } while (!config.Done);
       };
       await Task.Run(work);
    }

    static async void Example(int n)
    {
        // This method runs asynchronously.
        await Task.Run(() => Allocate(n));
        Console.WriteLine($"Total: {tot.Result.ToString()}.");
    }

    static void Allocate(int v)
    {
        Thread.Sleep(2000);
        tot.Result += v;
    }
}

public class Tot
{
    public int Result { get; set; } = 0;
}