using AutoMapper;
using DAL.Interface;
using DALEF.Context;
using DALEF.Models;
using DTO;
using System.Collections.Generic;
using System.Linq;

namespace DALEF.Concrete
{
    public class ProductDALEF : IProductDAL 
    {
        private readonly MyDatabaseContext _context; 
        private readonly IMapper _mapper; 

        public ProductDALEF(MyDatabaseContext context, IMapper mapper) 
        {
            _context = context; // Ініціалізація контексту бази даних
            _mapper = mapper; // Ініціалізація маппера
        }

        public List<Product> GetAll() // Отримання всіх продуктів
        {
            return _mapper.Map<List<Product>>(_context.TblProducts.ToList()); // Маппінг з TblProducts в DTO
        }

        public Product GetById(int id) // Отримання продукту за ID
        {
            var tblProduct = _context.TblProducts.Find(id); // Знаходження продукту за ID
            return _mapper.Map<Product>(tblProduct); // Маппінг з TblProduct в DTO
        }

        public Product Create(Product product) // Додавання нового продукту
        {
            var tblProduct = _mapper.Map<TblProduct>(product); // Маппінг з DTO в TblProduct
            _context.TblProducts.Add(tblProduct); // Додавання в контекст
            _context.SaveChanges(); // Збереження змін

            product.ProductId = tblProduct.product_id; // Присвоєння ID
            return product; // Повернення продукту
        }

        public Product Update(int id, Product product) // Оновлення продукту
        {
            var tblProduct = _context.TblProducts.Find(id); // Знаходження продукту за ID
            if (tblProduct == null) return null; // Перевірка на наявність продукту

            // Маппінг нових значень в tblProduct
            tblProduct.product_name= product.ProductName;
            tblProduct.price= product.Price;
            tblProduct.quantity= product.Quantity;
            tblProduct.category_id= product.CategoryId;
            tblProduct.user_id= product.UserId;

            _context.SaveChanges(); // Збереження змін
            return product; // Повернення оновленого продукту
        }

        public Product Delete(int id) // Видалення продукту
        {
            var tblProduct = _context.TblProducts.Find(id); // Знаходження продукту за ID
            if (tblProduct == null) return null; // Перевірка на наявність продукту

            _context.TblProducts.Remove(tblProduct); // Видалення з контексту
            _context.SaveChanges(); // Збереження змін
            return _mapper.Map<Product>(tblProduct); // Повернення видаленого продукту
        }
    }
}

