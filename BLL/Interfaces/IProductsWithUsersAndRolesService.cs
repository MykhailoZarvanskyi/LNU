using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace BLL.Interfaces
{
    public interface IProductsWithUsersAndRolesService
    {
        Task<IEnumerable<ProductsWithUsersAndRolesDTO>> GetAllProductsWithUsersAndRolesAsync();
    }
}
