namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class AlbumDAO
    {
        private string connectionString;

        public AlbumDAO(string dataConnection)
        {
            connectionString = dataConnection;
        }

        //Instatiating
        List<AlbumDO> albums = new List<AlbumDO>();


        //Create
        public void CreateAlbum(AlbumDO album)
        {
            try
            {
                //Opening Connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to create user
                    SqlCommand command = new SqlCommand("CREATE_ALBUM", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", album.UserId);
                    command.Parameters.AddWithValue("@AlbumName", album.AlbumName);
                    command.Parameters.AddWithValue("@AlbumDescription", album.AlbumDescription);

                    //Open connection and excecute nonquery
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //Read
        public List<AlbumDO> ReadAlbum()
        {
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all albums
                    SqlCommand command = new SqlCommand("READ_ALBUM", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    //Using adapter to get table from the datbase
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                        adapter.Dispose();
                    }
                    //Put datarow into a list of AlbumDO
                    foreach (DataRow row in dataTable.Rows)
                    {
                        AlbumDO mappedAlbum = MapAllAlbums(row);
                        albums.Add(mappedAlbum);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return albums;
        }


        //Update
        public void UpdateAlbum(AlbumDO album)
        {
            //Opening connection
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to update album
                    SqlCommand command = new SqlCommand("UPDATE_ALBUM", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AlbumID", album.AlbumId);
                    command.Parameters.AddWithValue("@UserID", album.UserId);
                    command.Parameters.AddWithValue("@AlbumName", album.AlbumName);
                    command.Parameters.AddWithValue("@AlbumDescription", album.AlbumDescription);

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
        public void DeleteAlbum(long album)
        {
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Using stored procedure to delete an album
                    SqlCommand command = new SqlCommand("DELETE_ALBUM", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AlbumID", album);

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


        //View album by albumid
        public AlbumDO ViewAlbumById(long album)
        {
            AlbumDO userAlbum = new AlbumDO();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("ALBUM_BY_ALBUMID", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@AlbumID", album);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }
                    //Putting datarow into a List of the album object.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        userAlbum.UserId = (long)row["UserID"];
                        userAlbum.AlbumName = row["AlbumName"].ToString();
                        userAlbum.AlbumDescription = row["AlbumDescription"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userAlbum;
        }


        //View album by userid
        public List<AlbumDO> ViewAlbumByUserId(long user)
        {
            List<AlbumDO> userAlbum = new List<AlbumDO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("ALBUMS_BY_USERID", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@UserID", user);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }
                    //Putting datarow into a List of the album object.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        AlbumDO mappedAlbum = MapAllAlbums(row);
                        userAlbum.Add(mappedAlbum);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userAlbum;
        }

        //Count photos by AlbumId
        public AlbumDO CountPhotos(long albumId)
        {
            AlbumDO count = new AlbumDO();
            try
            {
                //Creating connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all albums
                    SqlCommand command = new SqlCommand("COUNT_PHOTOS_BY_ALBUMID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AlbumId", albumId);

                    connection.Open();
                    command.ExecuteNonQuery();

                    //Using adapter to get table from the datbase
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                        adapter.Dispose();
                    }
                    //Put datarow into a list of PhotosDO
                    foreach (DataRow row in dataTable.Rows)
                    {
                        AlbumDO mappedCount = new AlbumDO();
                        mappedCount.PhotoCount = (int)row[0];
                        mappedCount.AlbumId = albumId;
                        count = mappedCount;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        //Map all the albums from a datarow to an AlbumDO
        public AlbumDO MapAllAlbums(DataRow row)
        {
            AlbumDO mappedAlbums = new AlbumDO();
            try
            {
                if (row["AlbumID"] != null)
                {
                    mappedAlbums.AlbumId = (long)row["AlbumID"];
                }
                mappedAlbums.AlbumName = row["AlbumName"].ToString();
                mappedAlbums.AlbumDescription = row["AlbumDescription"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mappedAlbums;
        }
    }
}
