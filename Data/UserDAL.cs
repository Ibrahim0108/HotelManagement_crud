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
    public class UserDAL
    {
        private readonly string connectionString;
        public UserDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["HotelDBConnection"].ConnectionString;
        }


        public User GetUserByUsername(string username)
        {
            User user = null;
            string sql = "SELECT UserID, Username, Password, Role, IsPendingAdmin FROM Users WHERE Username = @Username;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsPendingAdmin = Convert.ToBoolean(reader["IsPendingAdmin"])
                            };
                        }
                    }
                }
            }
            return user;
        }


        public void InsertUser(User user)
        {
            // Note: The Role is always 'User' here for security.
            string sql = "INSERT INTO [dbo].[Users] (Username, Password, Role, IsPendingAdmin) VALUES (@Username, @Password, @Role, @IsPendingAdmin);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    // In a real app, you must hash the password. For this example, we keep it simple.
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", "User"); // Default role is always 'User'
                    command.Parameters.AddWithValue("@IsPendingAdmin", user.IsPendingAdmin);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public User GetUser(string username, string password)
        {
            User user = null;
            string sql = "SELECT UserID, Username, Password, Role, IsPendingAdmin FROM Users WHERE Username = @Username AND Password = @Password;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsPendingAdmin = Convert.ToBoolean(reader["IsPendingAdmin"])
                            };
                        }
                    }
                }
            }
            return user;
        }

        // Checks if a username already exists to prevent duplicate accounts.
        public bool UsernameExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            string sql = "SELECT UserId, Username, Role, IsPendingAdmin FROM Users";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allUsers.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString(),
                            IsPendingAdmin = reader.GetBoolean(reader.GetOrdinal("IsPendingAdmin"))
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return allUsers;
        }

        public List<User> GetPendingAdminRequests()
        {
            List<User> pendingUsers = new List<User>();
            string sql = "SELECT UserID, Username FROM Users WHERE IsPendingAdmin = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pendingUsers.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString()
                            });
                        }
                    }
                }
            }
            return pendingUsers;
        }

        public void ApproveAdminRequest(int userId)
        {
            string sql = "UPDATE Users SET Role = 'Admin', IsPendingAdmin = 0 WHERE UserID = @UserID;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RejectAdminRequest(int userId)
        {
            string sql = "UPDATE Users SET IsPendingAdmin = 0 WHERE UserID = @UserID;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}