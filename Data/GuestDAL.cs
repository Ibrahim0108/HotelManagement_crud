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
    public class GuestDAL
    {
         private readonly string connectionString;
         public GuestDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["HotelDBConnection"].ConnectionString;
        }

         public Guest GetGuestById(int guestId)
         {
             Guest guest = null;
             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_SelectGuests", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     // The stored procedure's @GuestID parameter is used to filter the result.
                     cmd.Parameters.AddWithValue("@GuestID", guestId);
                     con.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     if (reader.Read())
                     {
                         guest = new Guest
                         {
                             GuestId = (int)reader["GuestID"],
                             FirstName = (string)reader["FirstName"],
                             LastName = (string)reader["LastName"],
                             Email = (string)reader["Email"],
                             PhoneNumber = reader["PhoneNumber"].ToString()
                         };
                     }
                 }
             }
             return guest;
         }

         public List<Guest> GetAllGuests()
         {
             List<Guest> guests = new List<Guest>();
             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_SelectGuests", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     // Passing DBNull.Value to @GuestID tells the stored procedure to return all guests.
                     cmd.Parameters.AddWithValue("@GuestID", DBNull.Value);
                     con.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     while (reader.Read())
                     {
                         guests.Add(new Guest
                         {
                             GuestId = (int)reader["GuestID"],
                             FirstName = (string)reader["FirstName"],
                             LastName = (string)reader["LastName"],
                             Email = (string)reader["Email"],
                             PhoneNumber = reader["PhoneNumber"].ToString()
                         });
                     }
                 }
             }
             return guests;
         }


         public void CreateGuest(Guest guest)
         {
             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_InsertGuest", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                     cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                     cmd.Parameters.AddWithValue("@Email", guest.Email);
                     // Handle the optional PhoneNumber parameter.
                     cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber ?? (object)DBNull.Value);
                     con.Open();
                     cmd.ExecuteNonQuery();
                 }
             }
         }

         public void UpdateGuest(Guest guest)
         {
             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_UpdateGuest", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@GuestID", guest.GuestId);
                     cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                     cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                     cmd.Parameters.AddWithValue("@Email", guest.Email);
                     // Handle the optional PhoneNumber parameter.
                     cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber ?? (object)DBNull.Value);
                     con.Open();
                     cmd.ExecuteNonQuery();
                 }
             }
         }

         public void DeleteGuest(int guestId)
         {
             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_DeleteGuest", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@GuestID", guestId);
                     con.Open();
                     cmd.ExecuteNonQuery();
                 }
             }
         }
    }
}