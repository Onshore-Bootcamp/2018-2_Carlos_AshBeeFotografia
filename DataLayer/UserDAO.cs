namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using DataLayer.Models;

    public class UserDAO
    {
        private string connectionString;

        public UserDAO(string dataConnection)
        {
            connectionString = dataConnection;
        }

        /// <summary>
        /// Create A User
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(UserDO user)
        {
            try
            {
                //Opening Connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to create user
                    SqlCommand command = new SqlCommand("CREATE_USER", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Firstname", user.Firstname);
                    command.Parameters.AddWithValue("@Lastname", user.Lastname);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Email", user.Email);

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


        //Read
        public List<UserDO> ReadUsers()
        {

            //Instatiating
            List<UserDO> users = new List<UserDO>();
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all users
                    SqlCommand command = new SqlCommand("READ_USER", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    //Using adapter to get table from the datbase
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                        adapter.Dispose();
                    }
                    //Put datarow into a list of UserDO
                    foreach (DataRow row in dataTable.Rows)
                    {
                        UserDO mappedUser = MapAllUsers(row);
                        users.Add(mappedUser);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return users;
        }


        //Update
        public void UpdateUser(UserDO user)
        {
            //Opening connection
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to update user
                    SqlCommand command = new SqlCommand("UPDATE_USER", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", user.UserId);
                    command.Parameters.AddWithValue("@RoleID", user.RoleId);
                    command.Parameters.AddWithValue("@Firstname", user.Firstname);
                    command.Parameters.AddWithValue("@Lastname", user.Lastname);
                    command.Parameters.AddWithValue("Username", user.Username);
                    command.Parameters.AddWithValue("Password", user.Password);
                    command.Parameters.AddWithValue("Phone", user.Phone);
                    command.Parameters.AddWithValue("Email", user.Email);

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


        //Delete
        public void DeleteUser(long userId)
        {
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Using stored procedure to delete a user
                    SqlCommand command = new SqlCommand("DELETE_USER", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

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


        //View by username
        public UserDO ViewUserByName(string username)
        {
            UserDO user = new UserDO();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("READ_BY_USERNAME", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@Username", username);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }
                    //Extract user information from datatable.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        user.UserId = (long)row["UserID"];
                        user.RoleId = (long)row["RoleID"];
                        user.Username = row["Username"].ToString();
                        user.Password = row["Password"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }

            return user;
        }


        //View by UserId
        public UserDO ViewUserById(long userId)
        {
            UserDO user = new UserDO();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("READ_BY_USERID", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }
                    //Putting datarow into a List of the user object.

                    //Use map method
                    foreach (DataRow row in dataTable.Rows)
                    {
                        user.RoleId = (long)row["RoleID"];
                        user.Firstname = row["FirstName"].ToString();
                        user.Lastname = row["LastName"].ToString();
                        user.Username = row["Username"].ToString();
                        user.Password = row["Password"].ToString();
                        user.Phone = row["Phone"].ToString();
                        user.Email = row["Email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        //Map users from database to a list
        public UserDO MapAllUsers(DataRow row)
        {
            UserDO mappedUsers = new UserDO();
            try
            {
                if (row["UserID"] != DBNull.Value)
                {
                    mappedUsers.UserId = (long)row["UserID"];
                }
                mappedUsers.RoleId = (long)row["RoleID"];
                mappedUsers.Firstname = row["Firstname"].ToString();
                mappedUsers.Lastname = row["Lastname"].ToString();
                mappedUsers.Username = row["Username"].ToString();
                mappedUsers.Password = row["Password"].ToString();
                mappedUsers.Phone = row["Phone"].ToString();
                mappedUsers.Email = row["Email"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mappedUsers;
        }

        public DataTable ReadAllExceptions()
        {

            DataTable dataTable = new DataTable();
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all users
                    SqlCommand command = new SqlCommand("READ_ALL_EXCEPTIONS", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    //Using adapter to get table from the datbase

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                        adapter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }
    }

}
