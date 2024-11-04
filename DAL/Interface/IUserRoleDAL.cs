using DTO;
using System.Collections.Generic;

namespace DAL.Interface
{
    public interface IUserRoleDAL
    {
        List<UserRole> GetAll(); 
        UserRole GetById(int id); 
        UserRole Create(UserRole userRole); 
        UserRole Update(int id, UserRole userRole); 
        UserRole Delete(int id); 

        UserRole GetByCredentials(string userName, string userPassword);
    }
}
