using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRoleDAL
{
    Task<UserRole> GetByCredentialsAsync(string username, string password); 
    Task<UserRole> GetByIdAsync(int id); 
    Task<List<UserRole>> GetAllAsync(); 
    Task<UserRole> CreateAsync(UserRole userRole); 
    Task<UserRole> UpdateAsync(int id, UserRole userRole); 
    Task<UserRole> DeleteAsync(int id); 
}

