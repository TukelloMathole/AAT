    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

## Code-Block: "using" Directives
This block includes necessary namespaces for handling collections (Concurrent, Collections.Generic), file operations (IO), LINQ (Linq), and threading (Threading).

    class Program
    {
        // ConcurrentQueue to store integers globally
        static ConcurrentQueue<int> globalList = new ConcurrentQueue<int>();
        // Counter for the number of odd integers processed
        static int oddCount = 0;
        // Counter for the number of even integers processed
        static int evenCount = 0;

## Code-Block: Global Variables

global variables:

globalList: A thread-safe queue (ConcurrentQueue<int>) to store integers.
oddCount: Counter for odd integers processed.
evenCount: Counter for even integers processed.


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
## Code-Block: Main Method - Thread Initialization
Sets up and starts three threads (oddThread, primeThread, evenThread) to generate random odd numbers, negative prime numbers, and random even numbers respectively. Each thread runs concurrently to populate globalList.
