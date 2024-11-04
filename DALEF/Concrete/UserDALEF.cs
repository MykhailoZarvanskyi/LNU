using AutoMapper;
using DAL.Interface;
using DALEF.Context;
using DALEF.Models;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DALEF.Concrete
{
    public class UserDALEF : IUserDAL 
    {
        private readonly MyDatabaseContext _context; 
        private readonly IMapper _mapper; 

        public UserDALEF(MyDatabaseContext context, IMapper mapper) 
        {
            _context = context; // Ініціалізація контексту бази даних
            _mapper = mapper; // Ініціалізація маппера
        }

        public List<User> GetAll() // Отримання всіх користувачів
        {
            return _mapper.Map<List<User>>(_context.TblUsers.ToList()); // Маппінг з TblUsers в DTO
        }

        public User GetById(int id) // Отримання користувача за ID
        {
            var tblUser = _context.TblUsers.Find(id); // Знаходження користувача за ID
            return _mapper.Map<User>(tblUser); // Маппінг з TblUser в DTO
        }

        public User Create(User user) // Додавання нового користувача
        {
            var tblUser = _mapper.Map<TblUser>(user); // Маппінг з DTO в TblUser
            _context.TblUsers.Add(tblUser); // Додавання в контекст
            _context.SaveChanges(); // Збереження змін

            user.UserId = tblUser.user_id; // Присвоєння ID
            return user; // Повернення користувача
        }

        public User Update(int id, User user) // Оновлення користувача
        {
            var tblUser = _context.TblUsers.Find(id); // Знаходження користувача за ID
            if (tblUser == null) return null; // Перевірка на наявність користувача

            // Маппінг нових значень в tblUser
            tblUser.user_name= user.UserName;
            tblUser.user_password= user.UserPassword;
            tblUser.role= user.RoleId;

            _context.SaveChanges(); // Збереження змін
            return user; // Повернення оновленого користувача
        }

        public User Delete(int id) // Видалення користувача
        {
            var tblUser = _context.TblUsers.Find(id); // Знаходження користувача за ID
            if (tblUser == null) return null; // Перевірка на наявність користувача

            _context.TblUsers.Remove(tblUser); // Видалення з контексту
            _context.SaveChanges(); // Збереження змін
            return _mapper.Map<User>(tblUser); // Повернення видаленого користувача
        }

        public User GetByCredentials(string userName, string userPassword) // Отримання користувача за логіном та паролем
        {
            var tblUser = _context.TblUsers
                .FirstOrDefault(u => u.user_name == userName && u.user_password == userPassword); // Пошук за логіном та паролем

            return _mapper.Map<User>(tblUser); // Маппінг з TblUser в DTO
        }
    }
}




