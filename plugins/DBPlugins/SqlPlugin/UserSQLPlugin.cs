using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace MattElandsBlog.plugins.DBPlugins.SqlPlugin
{
    public class UserSQLPlugin
    {

        [KernelFunction("get_schema")]
        [Description("Get Database schema.")]

        [return: Description("returns the Database schema")]
        public string GetSchema()
        {
            List<string> tables = new List<string> {
    "address","category","customer","customeraddresses","deliverydate","order","product",
    "shipment","store","warehouse"
    };
            string connectionString = "Server=BS-01216\\SQLEXPRESS01;Database=nop4.7;Trusted_Connection=True;TrustServerCertificate=True";

            // Define your query
            string query = @"SELECT      (t.name) AS 'TableName',
			    c.name  AS 'ColumnName',
                ty.name AS 'DataType'
            FROM        sys.columns c
            JOIN        sys.tables  t   ON c.object_id = t.object_id
            JOIN        sys.types   ty  ON c.user_type_id = ty.user_type_id
            ORDER BY    TableName,
            ColumnName;";
            string res2 = "";
            // Create a connection object
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a command object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the command and read the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            string prev = "nothingishere";
                            while (reader.Read())
                            {
                                StringBuilder rowString = new StringBuilder();
                                string tn = reader[0].ToString();
                                
                                if (tables.Contains(tn.ToLower()))
                                {
                                    if (tn == prev)
                                    {
                                        res2 += reader[1].ToString() + "(";
                                        res2 += reader[2].ToString() + "),";
                                    }
                                    else
                                    {
                                        res2 += "\ntable: " + reader[0].ToString() + '\n';
                                        res2 += reader[1].ToString() + "(";
                                        res2 += reader[2].ToString() + "),";
                                    }
                                    prev = tn;

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return res2;

        }


        [KernelFunction("execute_query")]

        [Description("Execute SQL Query .")]
        [return:Description("returns the query results by executing sql query")]
        public string ExecuteQuery(string query)
        {
            string connectionString = "Server=BS-01216\\SQLEXPRESS01;Database=nop4.7;Trusted_Connection=True;TrustServerCertificate=True";

            // Define your query
            // string query = "SELECT TOP 5 * FROM address ;";  // Change this to your desired query
            string res = "";
            // Create a connection object
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a command object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the command and read the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                StringBuilder rowString = new StringBuilder();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowString.Append(reader.GetName(i) + ": " + reader[i] + "; ");
                                }

                                // Trim the last semicolon and space
                                if (rowString.Length > 2)
                                {
                                    rowString.Length -= 2;
                                }

                                // Save the row string to a variable or process it further
                                res += rowString.ToString();
                                //Console.WriteLine(res);
                                res += '\n';
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return res;
        }
    }
}