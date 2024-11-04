using DAL.Interface;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Concrete
{
    public class ProductDal : IProductDAL
    {
        private readonly SqlConnection _connection;

        public ProductDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public Product Create(Product product)
        {
            try
            {
                _connection.Open();

                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO products (product_name, price, quantity, category_id, user_id) " +
                                          "OUTPUT INSERTED.product_id " + 
                                          "VALUES (@ProductName, @Price, @Quantity, @CategoryId, @UserId);";

                    command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = product.ProductName;
                    command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = product.UserId;

                    
                    product.ProductId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while inserting product: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return product;
        }

        public Product Delete(int id)
        {
            Product deletedProduct = GetById(id);
            if (deletedProduct == null)
                return null;

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM products WHERE product_id = @ProductId";
                command.Parameters.AddWithValue("@ProductId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

            return deletedProduct;
        }

        public List<Product> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT product_id, product_name, price, quantity, category_id, user_id FROM products";

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var products = new List<Product>();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["product_id"]),
                        ProductName = reader["product_name"].ToString(),
                        Price = Convert.ToDecimal(reader["price"]),
                        Quantity = Convert.ToInt32(reader["quantity"]),
                        CategoryId = Convert.ToInt32(reader["category_id"]),
                        UserId = Convert.ToInt32(reader["user_id"])
                    });
                }

                _connection.Close();
                return products;
            }
        }

        public Product GetById(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT product_id, product_name, price, quantity, category_id, user_id " +
                                      "FROM products WHERE product_id = @ProductId";
                command.Parameters.AddWithValue("@ProductId", id);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["product_id"]),
                        ProductName = reader["product_name"].ToString(),
                        Price = Convert.ToDecimal(reader["price"]),
                        Quantity = Convert.ToInt32(reader["quantity"]),
                        CategoryId = Convert.ToInt32(reader["category_id"]),
                        UserId = Convert.ToInt32(reader["user_id"])
                    };

                    _connection.Close();
                    return product;
                }

                _connection.Close();
                return null;
            }
        }

        public Product Update(int id, Product product)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE products SET product_name = @ProductName, price = @Price, " +
                                      "quantity = @Quantity, category_id = @CategoryId, user_id = @UserId " +
                                      "WHERE product_id = @ProductId";

                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@UserId", product.UserId);
                command.Parameters.AddWithValue("@ProductId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                return GetById(id);
            }
        }
    }
}
