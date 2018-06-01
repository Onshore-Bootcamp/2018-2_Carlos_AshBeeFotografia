namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;

    public class PhotosDAO
    {
        private string connectionString;

        public PhotosDAO(string dataConnection)
        {
            connectionString = dataConnection;
        }

        //Instatiating
        List<PhotosDO> photos = new List<PhotosDO>();


        //Create
        public void CreatePhoto(PhotosDO photo)
        {
            try
            {
                //Opening Connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to create photo.
                    SqlCommand command = new SqlCommand("CREATE_PHOTO", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AlbumID", photo.AlbumId);
                    command.Parameters.AddWithValue("@PhotoLocation", photo.PhotoLocation);
                    command.Parameters.AddWithValue("@PhotoName", photo.PhotoName);
                    command.Parameters.AddWithValue("@PhotoDate", photo.PhotoDate);
                    command.Parameters.AddWithValue("@Description", photo.Description);

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
        public List<PhotosDO> ReadPhotos()
        {
            //todo: Instanciate list here.
            try
            {
                //Creating connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all albums
                    SqlCommand command = new SqlCommand("READ_PHOTO", connection);
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
                        PhotosDO mappedPhotos = new PhotosDO();
                        mappedPhotos.AlbumId = (long)row[0];
                        photos.Add(mappedPhotos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photos;
        }


        //Update
        public void UpdatePhoto(PhotosDO photo)
        {
            //Opening connection
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to update photo
                    SqlCommand command = new SqlCommand("UPDATE_PHOTO", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PhotoID", photo.PhotoId);
                    command.Parameters.AddWithValue("@AlbumID", photo.AlbumId);
                    command.Parameters.AddWithValue("@PhotoLocation", photo.PhotoLocation);
                    command.Parameters.AddWithValue("@PhotoName", photo.PhotoName);
                    command.Parameters.AddWithValue("@PhotoDate", photo.PhotoDate);
                    command.Parameters.AddWithValue("@Description", photo.Description);

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
        public void DeletePhoto(long photo, string fullPath)
        {
            try
            {
                //Opening connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Using stored procedure to delete an photo
                    SqlCommand command = new SqlCommand("DELETE_PHOTO", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PhotoID", photo);

                    //Open connection and excecute nonquery
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                //Delete photo from file location.
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Map all photos from a datarow to an PhotosDO
        public PhotosDO MapAllPhotos(DataRow row)
        {
            PhotosDO mappedPhotos = new PhotosDO();
            try
            {
                mappedPhotos.AlbumId = (long)row["AlbumId"];
                mappedPhotos.PhotoId = (long)row["PhotoID"];
                mappedPhotos.PhotoLocation = row["PhotoLocation"].ToString();
                mappedPhotos.PhotoName = row["PhotoName"].ToString();
                mappedPhotos.PhotoDate = (DateTime)row["PhotoDate"];
                mappedPhotos.Description = row["Description"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mappedPhotos;
        }

        //View photos by albumId
        public List<PhotosDO> ViewPhotosById(long album)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("READ_PHOTO_BY_ALBUMID", connection);
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
                    //Putting datarow into a List of the photos object.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        PhotosDO mappedPhotos = MapAllPhotos(row);
                        photos.Add(mappedPhotos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photos;
        }

        //View photo by photoId
        public PhotosDO ViewPhotoByPhotoId(long photo)
        {
            PhotosDO photoInfo = new PhotosDO();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("READ_PHOTO_BY_PHOTOID", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@PhotoID", photo);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }
                    //Putting datarow into a List of the photos object.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        photoInfo.AlbumId = (long)row["AlbumID"];
                        photoInfo.PhotoLocation = row["PhotoLocation"].ToString();
                        photoInfo.PhotoName = row["PhotoName"].ToString();
                        photoInfo.PhotoDate = (DateTime)row["PhotoDate"];
                        photoInfo.Description = row["Description"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return photoInfo;
        }

        //View photoLocation by photoId
        public string ViewPhotoLocation(long photo)
        {
            string photos = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("READ_PHOTO_LOCATION", connection);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@PhotoID", photo);
                    connection.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(dataTable);
                        userAdapter.Dispose();
                    }

                    //Putting photo location into a string and formatting.                   
                    foreach (DataRow row in dataTable.Rows)
                    {
                        photos = row["PhotoLocation"].ToString();
                        photos = photos.TrimStart('~');
                        photos = string.Concat(@"C:\Users\Carlos\source\repos\AshBeeFotografia\AshBeeFotografia", photos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photos;
        }

        //Read
        public DataTable ReadPhotosForCount()
        {
            //todo: Instanciate list here.
            DataTable dataTable = new DataTable();
            try
            {
                //Creating connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Use stored procedure to view all albums
                    SqlCommand command = new SqlCommand("READ_PHOTO", connection);
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




