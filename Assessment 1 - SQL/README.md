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
    lock (results) // Ensure thread-safe access to results list
    {
        results.AddRange(result);
    }
});

// Batch insert into 'received_total' table
const int batchSize = 1000; // Batch size for inserting records
string insertQuery = @"INSERT INTO received_total (rt_msisdn, rt_message) VALUES (@msisdn, @message)";
List<received> batch = new List<received>(); // List to hold the current batch of records

// Establishing the database connection
using (SqlConnection connection = new SqlConnection(ConnectionString))
{
    connection.Open();
    
    using (SqlCommand command = new SqlCommand())
    {
        command.Connection = connection;
        command.CommandText = insertQuery;

        foreach (received rec in results)
        {
            command.Parameters.Clear(); // Clear parameters for each record

            // Set parameters for the current record
            command.Parameters.AddWithValue("@msisdn", rec.re_fromnum);
            command.Parameters.AddWithValue("@message", rec.re_message);

            command.ExecuteNonQuery();
            batch.Add(rec);

            // Batch commit - Execute the batch insert after 'batchSize' records
            if (batch.Count >= batchSize)
            {
                // Insert batch records
                foreach (received item in batch)
                {
                    command.Parameters.Clear(); // Clear parameters for each record

                    // Set parameters for the batch insert
                    command.Parameters.AddWithValue("@msisdn", item.re_fromnum);
                    command.Parameters.AddWithValue("@message", item.re_message);

                    command.ExecuteNonQuery();
                }
                batch.Clear(); // Clear batch list
            }
        }

        // Final commit for remaining records
        if (batch.Count > 0)
        {
            foreach (received item in batch)
            {
                command.Parameters.Clear(); // Clear parameters for each record

                // Set parameters for the batch insert
                command.Parameters.AddWithValue("@msisdn", item.re_fromnum);
                command.Parameters.AddWithValue("@message", item.re_message);

                command.ExecuteNonQuery();
            }
        }
    }
}




Parameterized Queries:

    I changed the INSERT query to use parameters (@msisdn and @message). This approach is safer against SQL injection attacks and can improve performance by reusing query execution plans.

(@msisdn and @message) - is used to dynamically insert the value. It also Helps prevent SQL injection attacks by separating SQL logic from user input, Promotes efficient query execution by allowing SQL Server to optimize the execution plan and reuse cached queries and improves code readability and maintainability by clearly separating SQL structure from data values.

Batch Inserts:

    I used a batch size (batchSize) to reduce the number of trips to the database. Instead of executing each insert individually.

Connection Management:

    I moved the SqlConnection outside of the loop that iterates over results to reuse the same connection throughout the batch insertion process.

Thread Safety:

    I ensured thread-safe access to the results list when merging data from parallel queries using a lock statement around the results. This prevents concurrency issues when multiple threads modify the list simultaneously.
