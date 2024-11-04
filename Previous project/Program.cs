using AutoMapper;
using DAL.Concrete;
using DALEF.Concrete;
using DTO;
using DALEF.Mapping;
using DALEF.MappingProfile;
using DALEF.Context;

string connStr = "Data Source=DESKTOP-UANF194;Initial Catalog=MyDatabase;Integrated Security=True;Encrypt=False;";
var config = new MapperConfiguration(c =>
{
    c.AddMaps(typeof(UserProfile).Assembly);
    c.AddMaps(typeof(ProductProfile).Assembly);
    c.AddMaps(typeof(CategoryProfile).Assembly);
});

IMapper mapper = config.CreateMapper();

// Меню выбора действия
void DisplayMenu()
{
    Console.WriteLine("1. Додати нову категорію");
    Console.WriteLine("2. Переглянути всі категорії");
    Console.WriteLine("3. Видалити категорію");
    Console.WriteLine("4. Додати продукт");
    Console.WriteLine("5. Переглянути всі продукти");
    Console.WriteLine("6. Видалити продукт");
    Console.WriteLine("7. Додати користувача");
    Console.WriteLine("8. Переглянути всіх користувачів");
    Console.WriteLine("9. Видалити користувача");
    Console.WriteLine("10. Додати роль");
    Console.WriteLine("11. Переглянути всі ролі");
    Console.WriteLine("12. Видалити роль");
    Console.WriteLine("l. Вийти");
}

// Метод для обробки вибору меню
void HandleMenuOption(int optionNumber)
{
    switch (optionNumber)
    {
        case 1: AddCategorie(); break;
        case 2: ListAllCategories(); break;
        case 3: DeleteCategory(); break;
        case 4: AddProduct(); break;
        case 5: ListAllProducts(); break;
        case 6: DeleteProduct(); break;
        case 7: AddUser(); break;
        case 8: ListAllUsers(); break;
        case 9: DeleteUser(); break;
        case 10: AddRole(); break;
        case 11: ListAllRoles(); break;
        case 12: DeleteRole(); break;
    }
}

// Основной цикл программы
do
{
    DisplayMenu();
    string selectedOption = Console.ReadLine()?.Trim().ToLower();

    if (int.TryParse(selectedOption, out int optionNumber) && optionNumber >= 1 && optionNumber <= 12)
    {
        HandleMenuOption(optionNumber);
    }
    else if (selectedOption == "l")
    {
        break; // Вихід з програми
    }
} while (true);



// Функції CRUD для Categorie
void AddCategorie()
{
    Console.WriteLine("Please enter Categorie Name:");
    string name = Console.ReadLine();

    Console.WriteLine("Please enter Categorie Description:");
    string description = Console.ReadLine();

    var categorieDal = new CategoryDAL(connStr); // Використовуємо твій клас CategorieDal
    var category = new Category { CategoryName = name, CategoryDescription = description };
    categorieDal.Create(category);

    Console.WriteLine($"Categorie '{name}' has been added.");
}


void ListAllCategories()
{
    var categorieDal = new CategoryDAL(connStr);
    List<Category> categories = categorieDal.GetAll();

    foreach (var category in categories)
    {
        Console.WriteLine($"{category.CategoryId}.\t{category.CategoryName}\t{category.CategoryDescription}");
    }
}

void DeleteCategory()
{
    Console.WriteLine("Please enter Categorie ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var categorieDal = new CategoryDAL(connStr);
    Category deletedCategory = categorieDal.Delete(id);

    if (deletedCategory != null)
        Console.WriteLine($"Categorie '{deletedCategory.CategoryName}' has been deleted.");
    else
        Console.WriteLine("Categorie not found.");
}


void AddRole()
{
    Console.WriteLine("Please enter Role Name:");
    string roleName = Console.ReadLine();

    Console.WriteLine("Please enter Role Description:");
    string roleDescription = Console.ReadLine();

    var roleDal = new RoleDAL(connStr);
    var role = new Role
    {
        RoleName = roleName,
        RoleDescription = roleDescription
    };
    roleDal.Create(role);

    Console.WriteLine($"Role '{roleName}' has been added.");
}

void ListAllRoles()
{
    var roleDal = new RoleDAL(connStr);
    List<Role> roles = roleDal.GetAll();
    foreach (var role in roles)
    {
        Console.WriteLine($"{role.RoleId}.\t{role.RoleName}\t{role.RoleDescription}");
    }
}

void DeleteRole()
{
    Console.WriteLine("Please enter Role ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var roleDal = new RoleDAL(connStr);
    Role deletedRole = roleDal.Delete(id);

    if (deletedRole != null)
        Console.WriteLine($"Role '{deletedRole.RoleName}' has been deleted.");
    else
        Console.WriteLine("Role not found.");
}

// Функції CRUD для Product
void AddProduct()
{
    Console.WriteLine("Please enter Product Name:");
    string productName = Console.ReadLine();

    Console.WriteLine("Please enter Product Price:");
    decimal price = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Please enter Product Quantity:");
    int quantity = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Category ID:");
    int categoryId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter User ID:");
    int userId = Convert.ToInt32(Console.ReadLine());

    var productDal = new ProductDal(connStr);
    var product = new Product
    {
        ProductName = productName,
        Price = price,
        Quantity = quantity,
        CategoryId = categoryId,
        UserId = userId
    };
    productDal.Create(product);

    Console.WriteLine($"Product '{productName}' has been added.");
}


void ListAllProducts()
{
    var productDal = new ProductDal(connStr);
    List<Product> products = productDal.GetAll();
    foreach (var product in products)
    {
        Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.Price}\t{product.Quantity}");
    }
}


void DeleteProduct()
{
    Console.WriteLine("Please enter Product ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var productDal = new ProductDal(connStr);
    Product deletedProduct = productDal.Delete(id);

    if (deletedProduct != null)
        Console.WriteLine($"Product '{deletedProduct.ProductName}' has been deleted.");
    else
        Console.WriteLine("Product not found.");
}


// Функції CRUD для User
void AddUser()
{
    Console.WriteLine("Please enter User Name:");
    string userName = Console.ReadLine();

    Console.WriteLine("Please enter User Password:");
    string userPassword = Console.ReadLine();

    Console.WriteLine("Please enter User Role ID:");
    int roleId = int.Parse(Console.ReadLine()); // Отримуємо RoleId як int

    var userRoleDal = new UserRoleDAL(connStr); // Використовуємо UserRoleDAL
    var userRole = new UserRole
    {
        UserName = userName,
        UserPassword = userPassword,
        RoleId = roleId
    };

    var createdUser = userRoleDal.Create(userRole);

    if (createdUser != null)
    {
        Console.WriteLine($"User '{userName}' has been added with Role '{createdUser.RoleName}'.");
    }
    else
    {
        Console.WriteLine("Error adding user.");
    }
}

void ListAllUsers()
{
    var userRoleDal = new UserRoleDAL(connStr);
    List<UserRole> users = userRoleDal.GetAll();

    if (users.Count == 0)
    {
        Console.WriteLine("No users found.");
        return;
    }

    foreach (var user in users)
    {
        Console.WriteLine($"{user.UserId}.\t{user.UserName}\t{user.RoleName}");
    }
}

void DeleteUser()
{
    Console.WriteLine("Please enter User ID to delete:");
    int userId;

    // Перевіряємо, чи введено правильне число
    while (!int.TryParse(Console.ReadLine(), out userId))
    {
        Console.WriteLine("Invalid input. Please enter a valid User ID:");
    }

    var userRoleDal = new UserRoleDAL(connStr);
    var deletedUserRole = userRoleDal.Delete(userId);

    if (deletedUserRole != null)
    {
        Console.WriteLine($"User '{deletedUserRole.UserName}' has been deleted.");
    }
    else
    {
        Console.WriteLine("Error deleting user. User may not exist.");
    }
}



// Функції CRUD для Categorie з використанням EF
/*
void AddCategorieWithEF()
{
    string name = "Empty";
    Console.WriteLine("Please enter Categorie Name:");
    name = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Category name cannot be empty.");
        return; // Вихід з функції, якщо ім'я не вказано
    }

    Console.WriteLine("Please enter Categorie Description:");
    string description = Console.ReadLine();

    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, conf.CreateMapper());
        var category = new Category { CategoryName = name, CategoryDescription = description };

        // Логування даних
        Console.WriteLine($"Adding category with Name: '{name}' and Description: '{description}'");

        
            categoryDal.Create(category);
            Console.WriteLine($"Categorie '{name}' has been added.");
        
            
        
    }
}



void ListAllCategoriesWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, mapper); // Використовуємо DAL з EF
        List<Category> categories = categoryDal.GetAll();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found.");
            return;
        }

        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}.\t{category.CategoryName}\t{category.CategoryDescription}");
        }
    }
}

void DeleteCategorieWithEF()
{
    Console.WriteLine("Please enter Categorie ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, mapper); // Використовуємо DAL з EF
        Category deletedCategory = categoryDal.Delete(id);

        if (deletedCategory != null)
            Console.WriteLine($"Categorie '{deletedCategory.CategoryName}' has been deleted.");
        else
            Console.WriteLine("Categorie not found.");
    }
}


void AddUserWithEF()
{
    Console.WriteLine("Please enter User Name:");
    string userName = Console.ReadLine();

    Console.WriteLine("Please enter User Password:");
    string userPassword = Console.ReadLine();

    Console.WriteLine("Please enter User Role:");
    string userRole = Console.ReadLine();

    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper); 
        var user = new User
        {
            UserName = userName,
            UserPassword = userPassword,
            RoleId = userRole
        };
        userDal.Create(user);

        Console.WriteLine($"User '{userName}' has been added.");
    }
}

void ListAllUsersWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper);
        List<User> users = userDal.GetAll();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"{user.UserId}.\t{user.UserName}\t{user.RoleId}");
        }
    }
}

void DeleteUserWithEF()
{
    Console.WriteLine("Please enter User ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper); 
        User deletedUser = userDal.Delete(id);

        if (deletedUser != null)
            Console.WriteLine($"User '{deletedUser.UserName}' has been deleted.");
        else
            Console.WriteLine("User not found.");
    }
}

void AddProductWithEF()
{
    Console.WriteLine("Please enter Product Name:");
    string productName = Console.ReadLine();

    Console.WriteLine("Please enter Product Price:");
    decimal price = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Please enter Product Quantity:");
    int quantity = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Category ID:");
    int categoryId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter User ID:");
    int userId = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        var product = new Product
        {
            ProductName = productName,
            Price = price,
            Quantity = quantity,
            CategoryId = categoryId,
            UserId = userId
        };
        productDal.Create(product);

        Console.WriteLine($"Product '{productName}' has been added.");
    }
}

void ListAllProductsWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        List<Product> products = productDal.GetAll();

        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.Price}\t{product.Quantity}");
        }
    }
}

void DeleteProductWithEF()
{
    Console.WriteLine("Please enter Product ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        Product deletedProduct = productDal.Delete(id);

        if (deletedProduct != null)
            Console.WriteLine($"Product '{deletedProduct.ProductName}' has been deleted.");
        else
            Console.WriteLine("Product not found.");
    }
}
*/






