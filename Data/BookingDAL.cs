using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HotelManagement.Data
{
    public class BookingDAL
    {
        private readonly string connectionString;

        public BookingDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["HotelDBConnection"].ConnectionString;
        }



        public List<Booking> GetAllBookings()
        {
            List<Booking> bookings = new List<Booking>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_SelectBookings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", DBNull.Value);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bookings.Add(new Booking
                        {
                            BookingId = (int)reader["BookingID"],
                            GuestId = (int)reader["GuestID"],
                            RoomID = (int)reader["RoomID"],
                            CheckInDate = (DateTime)reader["CheckInDate"],
                            CheckOutDate = (DateTime)reader["CheckOutDate"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            BookingDate = (DateTime)reader["BookingDate"],
                            RoomNumber = (string)reader["RoomNumber"],
                            RoomType = (string)reader["RoomType"],
                            GuestName = (string)reader["FirstName"] + " " + (string)reader["LastName"]
                        });
                    }
                }
            }
            return bookings;
        }


        public Booking GetBookingById(int bookingId)
        {
            Booking booking = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_SelectBookings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        booking = new Booking
                        {
                            BookingId = (int)reader["BookingID"],
                            GuestId = (int)reader["GuestID"],
                            RoomID = (int)reader["RoomID"],
                            CheckInDate = (DateTime)reader["CheckInDate"],
                            CheckOutDate = (DateTime)reader["CheckOutDate"],
                            // These fields have been added
                            TotalAmount = (decimal)reader["TotalAmount"],
                            BookingDate = (DateTime)reader["BookingDate"]
                        };
                    }
                }
            }
            return booking;
        }


        public void CreateBooking(Booking booking)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertBooking", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GuestID", booking.GuestId);
                    cmd.Parameters.AddWithValue("@RoomID", booking.RoomID);
                    cmd.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
                    // These parameters have been added
                    cmd.Parameters.AddWithValue("@TotalAmount", booking.TotalAmount);
                    cmd.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void UpdateBooking(Booking booking)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_UpdateBooking", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", booking.BookingId);
                    cmd.Parameters.AddWithValue("@GuestID", booking.GuestId);
                    cmd.Parameters.AddWithValue("@RoomID", booking.RoomID);
                    cmd.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
                    // These parameters have been added
                    cmd.Parameters.AddWithValue("@TotalAmount", booking.TotalAmount);
                    cmd.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteBooking(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DeleteBooking", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Updated method to reflect correct column name.
        public List<Room> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate)
        {
            List<Room> availableRooms = new List<Room>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAvailableRooms", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        availableRooms.Add(new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomNumber = (string)reader["RoomNumber"],
                            RoomType = (string)reader["RoomType"],
                            Price = (decimal)reader["Price"] // Corrected from PricePerNight
                        });
                    }
                }
            }
            return availableRooms;
        }

        // New method to retrieve a single room's details, including its price.
        public Room GetRoomById(int roomId)
        {
            Room room = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetRoomById", con)) // This stored procedure is new
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        room = new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomNumber = (string)reader["RoomNumber"],
                            RoomType = (string)reader["RoomType"],
                            Price = (decimal)reader["Price"] // Corrected column name
                        };
                    }
                }
            }
            return room;
        }


    }
}