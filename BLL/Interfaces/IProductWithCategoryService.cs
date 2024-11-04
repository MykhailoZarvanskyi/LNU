using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace DAL.Interfaces
{
    public interface IProductWithCategoryService
    {
        Task<IEnumerable<ProductsWithUsersAndRolesDTO>> GetProductsWithUsersAndRolesAsync();
        
    }
}
