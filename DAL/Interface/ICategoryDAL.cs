using DTO;

namespace DAL.Interface
{
     public interface ICategoryDAL
    {
        List<Category> GetAll(); 
        Category GetById(int id);
        Category Create(Category category); 
        Category Update(int id, Category category); 
        Category Delete(int id); 

    }
}
