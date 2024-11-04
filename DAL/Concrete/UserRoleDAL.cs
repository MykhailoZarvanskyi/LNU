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
    public class UserRoleDAL : IUserRoleDAL
    {
        private readonly string _connectionString;

        public UserRoleDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UserRole Create(UserRole userRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Вставка користувача в таблицю users
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO users (user_name, user_password, role_id) " +
                                          "OUTPUT INSERTED.user_id " +
                                          "VALUES (@UserName, @UserPassword, @RoleId);";

                    command.Parameters.AddWithValue("@UserName", userRole.UserName);
                    command.Parameters.AddWithValue("@UserPassword", userRole.UserPassword);
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userRole.UserId = (int)result;
                    }
                    else
                    {
                        throw new Exception("Failed to insert user into the database.");
                    }
                }

                // Отримуємо RoleName з таблиці roles для нового користувача
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT role_name FROM roles WHERE role_id = @RoleId";
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);

                    userRole.RoleName = command.ExecuteScalar()?.ToString();
                }
            }

            return userRole;
        }


        public UserRole Delete(int id)
        {
            UserRole deletedUserRole = GetById(id);
            if (deletedUserRole == null)
                return null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM users WHERE user_id = @UserId";
                    command.Parameters.AddWithValue("@UserId", id);
                    command.ExecuteNonQuery();
                }
            }
            return deletedUserRole;
        }

        public List<UserRole> GetAll()
        {
            var userRoles = new List<UserRole>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN dbo.roles r ON u.role_id = r.role_id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(new UserRole
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                UserName = reader["user_name"].ToString(),
                                UserPassword = reader["user_password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                RoleName = reader["role_name"].ToString()  
                            });
                        }
                    }
                }
            }

            return userRoles;
        }

        public UserRole GetByCredentials(string userName, string userPassword)
        {
            UserRole userRole = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN roles r ON u.role_id = r.role_id " +
                                          "WHERE u.user_name = @UserName AND u.user_password = @UserPassword";
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@UserPassword", userPassword); 

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userRole = new UserRole
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                UserName = reader["user_name"].ToString(),
                                UserPassword = reader["user_password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                RoleName = reader["role_name"].ToString()
                            };
                        }
                    }
                }
            }

            return userRole;
        }

        public UserRole GetById(int id)
        {
            UserRole userRole = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN roles r ON u.role_id = r.role_id WHERE u.user_id = @UserId";
                    command.Parameters.AddWithValue("@UserId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userRole = new UserRole
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                UserName = reader["user_name"].ToString(),
                                UserPassword = reader["user_password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),  
                                RoleName = reader["role_name"].ToString()
                            };
                        }
                    }
                }
            }

            return userRole;
        }

        public UserRole Update(int id, UserRole userRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE users SET user_name = @UserName, " +
                                          "user_password = @UserPassword, role_id = @RoleId " +
                                          "WHERE user_id = @UserId";

                    command.Parameters.AddWithValue("@UserName", userRole.UserName);
                    command.Parameters.AddWithValue("@UserPassword", userRole.UserPassword);
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);  
                    command.Parameters.AddWithValue("@UserId", id);

                    command.ExecuteNonQuery();
                }
            }
            return GetById(id);
        }
    }


}
