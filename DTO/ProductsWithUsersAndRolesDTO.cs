using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductsWithUsersAndRolesDTO
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } 
        public decimal Price { get; set; } 
        public int UserId { get; set; } 
        public string UserName { get; set; } 
        public string RoleName { get; set; } 
    }

}
