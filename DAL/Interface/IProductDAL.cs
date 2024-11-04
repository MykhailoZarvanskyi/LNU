using DTO;

namespace DAL.Interface
{
    public interface IProductDAL
    {
        List<Product> GetAll(); 
        Product GetById(int id); 
        Product Create(Product product); 
        Product Update(int id, Product product); 
        Product Delete(int id); 

    }
}
