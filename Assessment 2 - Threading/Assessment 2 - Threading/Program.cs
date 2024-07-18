﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    // ConcurrentQueue to store integers globally
    static ConcurrentQueue<int> globalList = new ConcurrentQueue<int>();
    // Counter for the number of odd integers processed
    static int oddCount = 0;
    // Counter for the number of even integers processed
    static int evenCount = 0;

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

        // this start thread for adding random odd numbers
        oddThread.Start();

        // Start thread for adding negative prime numbers
        primeThread.Start();


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

        // Serialize to binary and XML files
        SerializeToBinary(sortedList);
        SerializeToXml(sortedList);

        // Display results
        Console.WriteLine($"Total items in global list: {sortedList.Count}");
        Console.WriteLine($"Odd numbers: {oddCount}");
        Console.WriteLine($"Even numbers: {evenCount}");
        Console.WriteLine("Serialization completed.");

        Console.ReadLine(); // Keep console open
    }

    // Method to add random odd numbers to globalList
    static void AddRandomOddNumbers()
    {
        Random rand = new Random();
        // Continue adding numbers until globalList reaches 1 000 000 items
        while (globalList.Count < 1000000)
        {
            // Generating random number
            int num = rand.Next(1, int.MaxValue);
            // Check if the number is odd
            if (num % 2 != 0)
                globalList.Enqueue(num); // Add the odd number to globalList
        }
    }
    // Method to add negative prime numbers to globalList
    static void AddNegativePrimes()
    {
        int num = 2;
        // Check if a number is prime
        bool isPrime(int n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;
            int i = 5;
            while (i * i <= n)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
                i += 6;
            }
            return true;
        }
        // Continue adding negative prime numbers until globalList reaches 1 000 000 items
        while (globalList.Count < 1000000)
        {
            // Check if the current number is prime
            if (isPrime(num))
                globalList.Enqueue(-num); // Adding the negative prime number to globalList
            num++;
        }
    }

    // Method to add random even numbers to globalList
    static void AddRandomEvenNumbers()
    {
        Random rand = new Random();
        // Continue adding numbers until globalList reaches 1 000 000 items
        while (globalList.Count < 1000000)
        {
            // Generate a random even number
            int num = rand.Next(2, int.MaxValue);// Starting from 2 to ensure even numbers
            if (num % 2 == 0)
                globalList.Enqueue(num); // Adding the even number to globalList
        }
    }
    // Method to serialize a list of integers to a binary file
    static void SerializeToBinary(List<int> list)
    {
        // Concurrent to store byte arrays representing each integer
        var segments = new ConcurrentBag<byte[]>();
        // Converting each integer in the list to a byte array in parallel
        Parallel.ForEach(list, num =>
        {
            var buffer = BitConverter.GetBytes(num);
            segments.Add(buffer);// Adding the byte array to segments
        });
        // Writing all byte arrays from segments to a binary file
        using (FileStream fs = new FileStream("globalList.bin", FileMode.Create))
        {
            foreach (var segment in segments)
            {
                fs.Write(segment, 0, segment.Length); // Write the byte array to the file stream
            }
        }
    }
    // Method to serialize a list of integers to an XML file
    static void SerializeToXml(List<int> list)
    {
        object lockObject = new object(); // Object used for locking
        // Write to globalList.xml using StreamWriter
        using (StreamWriter sw = new StreamWriter("globalList.xml"))
        {
            sw.WriteLine("<GlobalList>");// Write the opening tag for GlobalList
            // Iterating through the list in parallel and write each number within a lock
            Parallel.ForEach(list, num =>
            {
                lock (lockObject)// Lock to ensure thread safety when writing to StreamWriter
                {
                    sw.WriteLine($"  <Number>{num}</Number>");// Write the number within Number tags
                }
            });

            sw.WriteLine("</GlobalList>");// Write the closing tag for GlobalList
        }
    }
}
