using AutoMapper;
using DAL.Concrete;
using DAL.Interface;
using DALEF.Context;
using DALEF.Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DALEF.Concrete
{
    public class CategoryDALEF : ICategoryDAL 
    {
        
        private readonly MyDatabaseContext _context;
        private readonly string _connectionstring;
        private readonly IMapper _mapper; 

        public CategoryDALEF(MyDatabaseContext context, IMapper mapper) 
        {
            _context = context; // Ініціалізація контексту бази даних
            _mapper = mapper; // Ініціалізація маппера
        }

        public CategoryDALEF(string connectionstring, IMapper mapper)
        {
            _connectionstring = connectionstring; // Ініціалізація контексту бази даних
            _mapper = mapper; // Ініціалізація маппера
        }

        public List<Category> GetAll() 
        {
            var tblCategories = _context.TblCategories.ToList(); // Отримання категорій з БД
            return _mapper.Map<List<Category>>(tblCategories); // Маппінг до DTO
        }




        public Category GetById(int id) // Отримання категорії за ID
        {
            var tblCategory = _context.TblCategories.Find(id); // Знаходження категорії за ID
            return _mapper.Map<Category>(tblCategory); // Маппінг з TblCategory в DTO
        }

        public Category Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("Category name cannot be null or empty");
            }

            var tblCategory = _mapper.Map<TblCategory>(category); // Маппінг з DTO в TblCategory

            // Логування перед збереженням
            Console.WriteLine($"Mapped Category: {tblCategory.category_name}, Description: {tblCategory.category_description}");

            _context.TblCategories.Add(tblCategory);
            _context.SaveChanges(); // Збереження змін

            category.CategoryId = tblCategory.category_id; // Присвоєння ID
            return category; // Повернення категорії
        }



        public Category Update(int id, Category category) // Оновлення категорії
        {
            var tblCategory = _context.TblCategories.Find(id); // Знаходження категорії за ID
            if (tblCategory == null) return null; // Перевірка на наявність категорії

            // Маппінг нових значень в tblCategory
            tblCategory.category_name = category.CategoryName;
            tblCategory.category_description = category.CategoryDescription;

            try
            {
                _context.SaveChanges(); // Збереження змін
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("An error occurred while updating the entity:");
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                return null; // або кидати виключення
            }

            return category; // Повернення оновленої категорії
        }


        public Category Delete(int id) // Видалення категорії
        {
            var tblCategory = _context.TblCategories.Find(id); // Знаходження категорії за ID
            if (tblCategory == null) return null; // Перевірка на наявність категорії

            _context.TblCategories.Remove(tblCategory); // Видалення з контексту
            _context.SaveChanges(); // Збереження змін
            return _mapper.Map<Category>(tblCategory); // Повернення видаленої категорії
        }

    }
}
