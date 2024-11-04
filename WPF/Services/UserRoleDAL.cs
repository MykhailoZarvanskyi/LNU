using DAL.Interface;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class UserRoleDAL : IUserRoleDAL
    {
        private readonly string _connectionString;

        public UserRoleDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserRole> GetByCredentialsAsync(string username, string password)
        {
            UserRole userRole = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN roles r ON u.role_id = r.role_id " +
                                          "WHERE u.user_name = @UserName AND u.user_password = @UserPassword";
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@UserPassword", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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

        public async Task<UserRole> GetByIdAsync(int id)
        {
            UserRole userRole = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN roles r ON u.role_id = r.role_id WHERE u.user_id = @UserId";
                    command.Parameters.AddWithValue("@UserId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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

        public async Task<List<UserRole>> GetAllAsync()
        {
            var userRoles = new List<UserRole>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT u.user_id, u.user_name, u.user_password, u.role_id, r.role_name " +
                                          "FROM users u JOIN roles r ON u.role_id = r.role_id";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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

        public async Task<UserRole> CreateAsync(UserRole userRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO users (user_name, user_password, role_id) " +
                                          "OUTPUT INSERTED.user_id " +
                                          "VALUES (@UserName, @UserPassword, @RoleId);";

                    command.Parameters.AddWithValue("@UserName", userRole.UserName);
                    command.Parameters.AddWithValue("@UserPassword", userRole.UserPassword);
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);

                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        userRole.UserId = (int)result;
                    }
                    else
                    {
                        throw new Exception("Failed to insert user into the database.");
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT role_name FROM roles WHERE role_id = @RoleId";
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);

                    userRole.RoleName = (await command.ExecuteScalarAsync())?.ToString();
                }
            }

            return userRole;
        }

        public async Task<UserRole> UpdateAsync(int id, UserRole userRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE users SET user_name = @UserName, " +
                                          "user_password = @UserPassword, role_id = @RoleId " +
                                          "WHERE user_id = @UserId";

                    command.Parameters.AddWithValue("@UserName", userRole.UserName);
                    command.Parameters.AddWithValue("@UserPassword", userRole.UserPassword);
                    command.Parameters.AddWithValue("@RoleId", userRole.RoleId);
                    command.Parameters.AddWithValue("@UserId", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return await GetByIdAsync(id);
        }

        public async Task<UserRole> DeleteAsync(int id)
        {
            UserRole deletedUserRole = await GetByIdAsync(id);
            if (deletedUserRole == null)
                return null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM users WHERE user_id = @UserId";
                    command.Parameters.AddWithValue("@UserId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return deletedUserRole;
        }
    }
}
