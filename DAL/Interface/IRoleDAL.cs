using DTO;
using System.Collections.Generic;

namespace DAL.Interface
{
    public interface IRoleDAL
    {
        List<Role> GetAll(); 
        Role GetById(int id); 
        Role Create(Role role); 
        Role Update(int id, Role role); 
        Role Delete(int id); 
    }
}

