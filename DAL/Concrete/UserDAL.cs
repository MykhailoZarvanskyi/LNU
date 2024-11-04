using DAL.Interface;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Concrete
{
    public class UserDal : IUserDAL
    {
        private readonly SqlConnection _connection;

        public UserDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public User Create(User user)
        {
            try
            {
                _connection.Open();

                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO users (user_name, user_password, role_id) " +
                                          "OUTPUT INSERTED.user_id " +
                                          "VALUES (@UserName, @UserPassword, @RoleId);";

                    
                    command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = user.UserName;
                    command.Parameters.Add("@UserPassword", SqlDbType.NVarChar).Value = HashPassword(user.UserPassword);
                    command.Parameters.Add("@RoleId", SqlDbType.NVarChar).Value = user.RoleId;

                    
                    user.UserId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while inserting user: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return user;
        }

        public User Delete(int id)
        {
            User deletedUser = GetById(id);
            if (deletedUser == null)
                return null;

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM users WHERE user_id = @UserId";
                command.Parameters.AddWithValue("@UserId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

            return deletedUser;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            try
            {
                _connection.Open();

                using (SqlCommand command = _connection.CreateCommand())
                {
                    
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u " +
                                          "JOIN roles r ON u.role_id = r.role_id";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                UserName = reader["user_name"].ToString(),
                                UserPassword = reader["user_password"].ToString(),
                                RoleId = reader["role_id"].ToString(),       
                                RoleName = reader["role_name"].ToString()    
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while retrieving users: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return users;
        }


        public User GetById(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                
                command.CommandText = "SELECT user_id, user_name, user_password, role_id FROM users WHERE user_id = @UserId";
                command.Parameters.AddWithValue("@UserId", id);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        UserName = reader["user_name"].ToString(),
                        UserPassword = reader["user_password"].ToString(),
                        RoleId = reader["role_id"].ToString() 
                    };

                    _connection.Close();
                    return user;
                }

                _connection.Close();
                return null;
            }
        }


        public User GetByCredentials(string userName, string userPassword)
        {
            try
            {
                _connection.Open();

                
                string hashedPassword = HashPassword(userPassword);

                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT user_id, user_name, user_password, role_id, role_name FROM UserRoles " +
                                          "WHERE user_name = @UserName AND user_password = @UserPassword";
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@UserPassword", hashedPassword); 

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var user = new User
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            UserName = reader["user_name"].ToString(),
                            UserPassword = reader["user_password"].ToString(),
                            RoleId = reader["role_id"].ToString(), 
                            RoleName = reader["role_name"].ToString() 
                        };

                        return user;
                    }

                    return null;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while retrieving user: {ex.Message}");
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); 
                }
                return builder.ToString();
            }
        }


        public User Update(int id, User user)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE users SET user_name = @UserName, user_password = @UserPassword, role_id = @RoleId WHERE user_id = @UserId";

                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);
                command.Parameters.AddWithValue("@RoleId", user.RoleId);
                command.Parameters.AddWithValue("@UserId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                return GetById(id);
            }
        }
    }
}
