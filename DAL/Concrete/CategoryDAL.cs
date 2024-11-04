using DAL.Interface;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Concrete
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly SqlConnection _connection;

        public CategoryDAL(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public Category Create(Category category)
        {
            try
            {
                _connection.Open();

                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Categories (category_name, category_description) " +
                                          "OUTPUT INSERTED.category_id " + 
                                          "VALUES (@CategoryName, @CategoryDescription);";

                    
                    command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = category.CategoryName;
                    command.Parameters.Add("@CategoryDescription", SqlDbType.NVarChar).Value = category.CategoryDescription;

                    
                    category.CategoryId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while inserting category: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return category;
        }

        public Category Delete(int id)
        {
            Category deletedCategory = GetById(id);
            if (deletedCategory == null)
                return null;

            _connection.Open();
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Categories WHERE category_id = @CategoryId";
                command.Parameters.AddWithValue("@CategoryId", id);
                command.ExecuteNonQuery();
            }
            _connection.Close();

            return deletedCategory;
        }

        public List<Category> GetAll()
        {
            var categories = new List<Category>();

            _connection.Open();
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT category_id, category_name, category_description FROM Categories";
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = Convert.ToInt32(reader["category_id"]),
                        CategoryName = reader["category_name"].ToString(),
                        CategoryDescription = reader["category_description"].ToString()
                    });
                }
            }
            _connection.Close();

            return categories;
        }

        public Category GetById(int id)
        {
            Category category = null;

            _connection.Open();
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT category_id, category_name, category_description " +
                                      "FROM Categories WHERE category_id = @CategoryId";
                command.Parameters.AddWithValue("@CategoryId", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    category = new Category
                    {
                        CategoryId = Convert.ToInt32(reader["category_id"]),
                        CategoryName = reader["category_name"].ToString(),
                        CategoryDescription = reader["category_description"].ToString()
                    };
                }
            }
            _connection.Close();

            return category;
        }

        public Category Update(int id, Category category)
        {
            _connection.Open();
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Categories SET category_name = @CategoryName, " +
                                      "category_description = @CategoryDescription " +
                                      "WHERE category_id = @CategoryId";

                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@CategoryDescription", category.CategoryDescription);
                command.Parameters.AddWithValue("@CategoryId", id);

                command.ExecuteNonQuery();
            }
            _connection.Close();

            return GetById(id);
        }
    }
}
