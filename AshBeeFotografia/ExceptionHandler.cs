namespace AshBeeFotografia
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class ExceptionHandler
    {
        private string connectionString;

        public ExceptionHandler(string connection)
        {
            connectionString = connection;
        }

        public void ExceptionLog(string level, string message, string currentClass, string currentMethod, string stackTrace = null)
        {
            try
            {
                //Opening Connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to create user
                    SqlCommand command = new SqlCommand("LOG_EXCEPTION", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Level", level);
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString());
                    command.Parameters.AddWithValue("@CurrentClass", currentClass);
                    command.Parameters.AddWithValue("@CurrentMethod", currentMethod);
                    command.Parameters.AddWithValue("@StackTrace", stackTrace);

                    //Open connection and excecute nonquery
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

