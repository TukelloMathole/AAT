Optimizations for Efficient Database Operations

Code Snippet:


    string sql = $"SELECT TOP 1000000 * FROM received WHERE status = 1 ORDER BY re_ref";

    // List of SQL nodes to query
    IEnumerable<IConfigurationSection> SqlNodes = Program.Configuration.GetSection("ConnectionStrings").GetSection("SqlNodes").GetChildren();

    // Merged result set
    List<received> results = new List<received>();

    // Using Parallel.ForEach to concurrently query databases
    Parallel.ForEach(SqlNodes, Node =>
    {
        received[] result = DBQuery<received>.Query(Node.Value, sql); // Internal function to query DB and return results
        lock (results) // Ensure thread is safe access to results list with the use of lock statement.
        {
            results.AddRange(result);
        }
    });

    // Batch insert into 'received_total' table
    string insertQuery = @"INSERT INTO received_total (rt_msisdn, rt_message) VALUES (@msisdn, @message)";
    const int batchSize = 1000; // Batch size for inserting records
    int recordsInserted = 0; // Initializing a counter for tracking the number of records successfully inserted into the database

    // Establishing the database connection
    using (SqlConnection connection = new SqlConnection(ConnectionString))
    {
        connection.Open();
        // Preparing SQL command for data insertion
        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            foreach (received rec in results)
            {
                command.Parameters.Clear(); // Clearing parameters from previous iteration with new ones

                // Set parameters for the current record
                command.Parameters.AddWithValue("@msisdn", rec.re_fromnum);
                command.Parameters.AddWithValue("@message", rec.re_message);

                command.ExecuteNonQuery();
                recordsInserted++;

                // Batch commit - Execute the batch insert after 'batchSize' records
                if (recordsInserted % batchSize == 0)
                {
                    command.ExecuteNonQuery(); // Executing the batch insert
                }
            }

            // Final commit for remaining records
            if (recordsInserted % batchSize != 0)
            {
                command.ExecuteNonQuery();
            }
        }
        connection.Close();// closing the connection
    }



Explanation of Changes:

Parameterized Queries:
        I changed the INSERT query to use parameters (@msisdn and @message). This approach is safer against SQL injection attacks and can improve performance by reusing query execution plans.
        (@msisdn and @message) - is used to dynamically insert the value. It also Helps prevent SQL injection attacks by separating SQL logic from user input, Promotes efficient query execution by allowing SQL Server to optimize the execution plan and reuse cached queries and improves code readability and maintainability by clearly separating SQL structure from data values.
Batch Inserts:
        I used a batch size (batchSize) to reduce the number of trips to the database. Instead of executing each insert individually.
Connection Management:
        I moved the SqlConnection outside of the loop that iterates over results to reuse the same connection throughout the batch insertion process.
Thread Safety:
        I ensured thread-safe access to the results list when merging data from parallel queries using a lock statement around the results. This prevents concurrency issues when multiple threads modify the list simultaneously.
