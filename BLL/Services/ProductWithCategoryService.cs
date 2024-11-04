using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL.Interfaces;
using DTO;

namespace DAL
{
    public class ProductWithCategoryDAL : IProductWithCategoryService
    {
        private readonly string _connectionString;

        public ProductWithCategoryDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ProductsWithUsersAndRolesDTO>> GetProductsWithUsersAndRolesAsync()
        {
            var productsWithUsersAndRoles = new List<ProductsWithUsersAndRolesDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(
                    @"SELECT p.ProductId, p.ProductName, p.Price, 
                             u.UserId, u.UserName, r.RoleName
                      FROM Products p
                      INNER JOIN Users u ON p.UserId = u.UserId
                      INNER JOIN Roles r ON u.RoleId = r.RoleId", connection))
                {
                    command.CommandType = CommandType.Text;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dto = new ProductsWithUsersAndRolesDTO
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
                            };

                            productsWithUsersAndRoles.Add(dto);
                        }
                    }
                }
            }

            return productsWithUsersAndRoles;
        }
    }
}
