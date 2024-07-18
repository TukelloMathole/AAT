## Code-Block: "using" Directives
This block includes necessary namespaces for handling collections (Concurrent, Collections.Generic), file operations (IO), LINQ (Linq), and threading (Threading).

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

## Code-Block: Global Variables

global variables:

globalList: A thread-safe queue (ConcurrentQueue<int>) to store integers.
oddCount: Counter for odd integers processed.
evenCount: Counter for even integers processed.

    class Program
    {
        // ConcurrentQueue to store integers globally
        static ConcurrentQueue<int> globalList = new ConcurrentQueue<int>();
        // Counter for the number of odd integers processed
        static int oddCount = 0;
        // Counter for the number of even integers processed
        static int evenCount = 0;


## Code-Block: Main Method - Thread Initialization
Sets up and starts three threads (oddThread, primeThread, evenThread) to generate random odd numbers, negative prime numbers, and random even numbers respectively. Each thread runs concurrently to populate globalList.

    static void Main(string[] args)
    {
        Console.WriteLine("starting program");
    
        // Create and start threads
    
        // This thread to add random odd numbers
        Thread oddThread = new Thread(AddRandomOddNumbers);
    
        // This thread to add negative prime numbers
        Thread primeThread = new Thread(AddNegativePrimes);
    
        // This thread to add random even numbers
        Thread evenThread = new Thread(AddRandomEvenNumbers);
    
        // Start thread for adding random odd numbers
        oddThread.Start();
    
        // Start thread for adding negative prime numbers
        primeThread.Start();

## Code-Block: Main Method - Thread Execution Control
Waits until globalList reaches specific counts (250,000 and 1,000,000) before starting and stopping threads. Uses Thread.Sleep for polling and Thread.Join for synchronization.

        while (globalList.Count < 250000)
        {
            Thread.Sleep(100); // Sleep for 100 milliseconds before checking again
        }
        
        // Start thread for adding negative prime numbers
        evenThread.Start();
        
        while (globalList.Count != 1000000)
        {
            Thread.Sleep(100); // Sleep for 100 milliseconds before checking again
        }
        
        // Stoping all threads
        oddThread.Join();
        primeThread.Join();
        evenThread.Join();

# Code-Block: Main Method - Processing
Explanation: Sorts globalList, then counts odd and even numbers using Parallel.ForEach for parallel iteration. Uses Interlocked.Increment for thread-safe increments of oddCount and evenCount.


       // Sorting the global list
        List<int> sortedList = globalList.ToList();
        sortedList.Sort();
    
        // Count odd and even numbers
        // Iterate through sortedList in parallel
        Parallel.ForEach(sortedList, num =>
        {
            // Check if the number is even
            if (num % 2 == 0)
            {
                // Increment evenCount atomically using Interlocked.Increment
                Interlocked.Increment(ref evenCount);
            }
            else
            {
                // Increment oddCount atomically using Interlocked.Increment
                Interlocked.Increment(ref oddCount);
            }
        });

# Code-Block: Main Method - Serialization and Output
Serializes sortedList to binary and XML files using SerializeToBinary and SerializeToXml methods. Displays counts of total items, odd numbers, and even numbers processed. Keeps console open with Console.ReadLine().

    // Serialize to binary and XML files
    SerializeToBinary(sortedList);
    SerializeToXml(sortedList);

    // Display results
    Console.WriteLine($"Total items in global list: {sortedList.Count}");
    Console.WriteLine($"Odd numbers: {oddCount}");
    Console.WriteLine($"Even numbers: {evenCount}");
    Console.WriteLine("Serialization completed.");

    Console.ReadLine(); // Keep console open


