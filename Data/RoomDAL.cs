using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HotelManagement.Models;

namespace HotelManagement.Data
{
    public class RoomDAL
    {
        private string connectionString;

        public RoomDAL()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["HotelDBConnection"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving connection string from Web.config. Please ensure 'HotelDBConnection' is defined correctly: " + ex.Message);
            }
        }

        public void InsertRoom(Room room)
        {
            using(SqlConnection connection  = new SqlConnection(connectionString))
            {
                 using (SqlCommand command = new SqlCommand("usp_InsertRoom", connection))
                 {
                     command.CommandType = CommandType.StoredProcedure;
                     command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@RoomType", room.RoomType);
                    command.Parameters.AddWithValue("@Price", room.Price);
                    command.Parameters.AddWithValue("@IsAvailable", room.IsAvailable);
                    command.Parameters.AddWithValue("@MainImageName", room.MainImageName ?? (object)DBNull.Value); // Handle nulls
                    command.Parameters.AddWithValue("@GalleryImageNames", room.GalleryImageNames ?? (object)DBNull.Value);

                     connection.Open();
                     command.ExecuteNonQuery();
                 }
            }
        }


       
        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>(); // A list to hold Room objects retrieved from the database.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_SelectRooms", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    // SqlDataReader: Provides a fast, forward-only stream of data rows from the database.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Loop through each row returned by the stored procedure.
                        {
                            // Create a new Room object for each row and populate its properties.
                            rooms.Add(new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]), 
                                RoomNumber = reader["RoomNumber"].ToString(), 
                                RoomType = reader["RoomType"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                                MainImageName = reader["MainImageName"] != DBNull.Value ? reader["MainImageName"].ToString() : null,
                                GalleryImageNames = reader["GalleryImageNames"] != DBNull.Value ? reader["GalleryImageNames"].ToString() : null
                            });
                        }
                    }
                }
            }
            return rooms; // Return the list of rooms.
        }


        public Room GetRoomById(int RoomID)
        {
            Room room = null;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand command = new SqlCommand("usp_SelectRooms",connection))
                {
                    command.CommandType =  CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoomID",RoomID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            room =  new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                RoomNumber = reader["RoomNumber"].ToString(),
                                RoomType = reader["RoomType"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                                MainImageName = reader["MainImageName"] != DBNull.Value ? reader["MainImageName"].ToString() : null,
                                GalleryImageNames = reader["GalleryImageNames"] != DBNull.Value ? reader["GalleryImageNames"].ToString() : null
                            };
                        }
                    }

                }
            }
            return room;
        }


        public void UpdateRoom(Room room)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand command =  new SqlCommand("usp_UpdateRoom",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoomID", room.RoomID);
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@RoomType", room.RoomType);
                    command.Parameters.AddWithValue("@Price", room.Price);
                    command.Parameters.AddWithValue("@IsAvailable", room.IsAvailable);
                    command.Parameters.AddWithValue("@MainImageName", room.MainImageName ?? (object)DBNull.Value); // Handle nulls
                    command.Parameters.AddWithValue("@GalleryImageNames", room.GalleryImageNames ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteRoom(int roomID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteRoom", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoomID", roomID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}