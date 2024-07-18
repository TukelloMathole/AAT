    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

Code-Block: "using" Directives
This block includes necessary namespaces for handling collections (Concurrent, Collections.Generic), file operations (IO), LINQ (Linq), and threading (Threading).

    class Program
    {
        // ConcurrentQueue to store integers globally
        static ConcurrentQueue<int> globalList = new ConcurrentQueue<int>();
        // Counter for the number of odd integers processed
        static int oddCount = 0;
        // Counter for the number of even integers processed
        static int evenCount = 0;

Block: Global Variables
Explanation: Defines global variables:

globalList: A thread-safe queue (ConcurrentQueue<int>) to store integers.
oddCount: Counter for odd integers processed.
evenCount: Counter for even integers processed.
