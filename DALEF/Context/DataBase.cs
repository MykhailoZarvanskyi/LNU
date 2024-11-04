using DALEF.Models;
using Microsoft.EntityFrameworkCore;

namespace DALEF.Context
{
    public class DataBase : MyDatabaseContext
    {
        private readonly string _connectionString;

        public DataBase(string connectionString)
            : base(new DbContextOptionsBuilder<MyDatabaseContext>()
                .UseSqlServer(connectionString)
                .Options) // Викликаємо базовий конструктор
        {
            _connectionString = connectionString;
        }

    }
}




