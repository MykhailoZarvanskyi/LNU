using DAL.Interface;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Concrete
{
    public class RoleDAL : IRoleDAL
    {
        private readonly SqlConnection _connection;

        public RoleDAL(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public Role Create(Role role)
        {
            try
            {
                _connection.Open();

                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO roles (role_name, role_description) " +
                                          "OUTPUT INSERTED.role_id " +
                                          "VALUES (@RoleName, @RoleDescription);";

                    command.Parameters.AddWithValue("@RoleName", role.RoleName);
                    command.Parameters.AddWithValue("@RoleDescription", role.RoleDescription);

                    
                    role.RoleId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while inserting role: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return role;
        }

        public Role Delete(int id)
        {
            Role deletedRole = GetById(id);
            if (deletedRole == null)
                return null;

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM roles WHERE role_id = @RoleId";
                command.Parameters.AddWithValue("@RoleId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

            return deletedRole;
        }

        public List<Role> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT role_id, role_name, role_description FROM roles";

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var roles = new List<Role>();
                while (reader.Read())
                {
                    roles.Add(new Role
                    {
                        RoleId = Convert.ToInt32(reader["role_id"]),
                        RoleName = reader["role_name"].ToString(),
                        RoleDescription = reader["role_description"].ToString()
                    });
                }

                _connection.Close();
                return roles;
            }
        }

        public Role GetById(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT role_id, role_name, role_description " +
                                      "FROM roles WHERE role_id = @RoleId";
                command.Parameters.AddWithValue("@RoleId", id);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var role = new Role
                    {
                        RoleId = Convert.ToInt32(reader["role_id"]),
                        RoleName = reader["role_name"].ToString(),
                        RoleDescription = reader["role_description"].ToString()
                    };

                    _connection.Close();
                    return role;
                }

                _connection.Close();
                return null;
            }
        }

        public Role Update(int id, Role role)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE roles SET role_name = @RoleName, role_description = @RoleDescription " +
                                      "WHERE role_id = @RoleId";

                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@RoleDescription", role.RoleDescription);
                command.Parameters.AddWithValue("@RoleId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                return GetById(id);
            }
        }
    }
}

