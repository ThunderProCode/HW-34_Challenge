using System;
using System.Threading;

public class RecentNumbers
{
    private int lastNumber;
    private int secondLastNumber;

    public void UpdateRecentNumbers(int number)
    {
        secondLastNumber = lastNumber;
        lastNumber = number;
    }

    public bool AreLastTwoNumbersSame()
    {
        return lastNumber == secondLastNumber;
    }
}

public class Program
{
    static RecentNumbers recentNumbers = new RecentNumbers();

    static void GenerateNumbers()
    {
        Random random = new Random();

        while (true)
        {
            int number = random.Next(0, 10);
            Console.WriteLine($"Generated number: {number}");

            // Update recent numbers
            lock (recentNumbers)
            {
                recentNumbers.UpdateRecentNumbers(number);
            }

            Thread.Sleep(1000); // Wait for one second
        }
    }

    static void Main()
    {
        // Start the number generation thread
        Thread numberGenerationThread = new Thread(GenerateNumbers);
        numberGenerationThread.Start();

        // Wait for the user to push a key
        Console.WriteLine("Press any key to flag a repeat...");
        while (true)
        {
            Console.ReadKey(true);

            // Check if the last two numbers are the same
            bool isRepeat = false;
            lock (recentNumbers)
            {
                isRepeat = recentNumbers.AreLastTwoNumbersSame();
            }

            if (isRepeat)
            {
                Console.WriteLine("You correctly identified the repeat!");
            }
            else
            {
                Console.WriteLine("You got it wrong.");
            }
        }
    }
}
