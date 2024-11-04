using DTO;

namespace DAL.Interface
{
    public interface IUserDAL
    {
        List<User> GetAll(); 
        User GetById(int id); 
        User Create(User user); 
        User Update(int id, User user); 
        User Delete(int id);

        // Метод для перевірки логіну та пароля
        User GetByCredentials(string userName, string userPassword);
    }
}
