using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfApp2.Services;
using WpfApp2.ViewModels;
using DAL.Interface;
using DAL.Concrete;

namespace WpfApp2
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureServices(); 

            try
            {
                
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while starting the application: {ex.Message}");
            }
        }

        private void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Реєстрація служб
            string connectionString = "Data Source=DESKTOP-UANF194;Initial Catalog=MyDatabase;Integrated Security=True;Encrypt=False;"; // Ваш рядок підключення до БД

            // Реєстрація IUserRoleDAL як Singleton
            serviceCollection.AddSingleton<IUserRoleDAL>(provider => new UserRoleDAL(connectionString));
            serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>(); // Реєстрація сервісу аутентифікації
            serviceCollection.AddTransient<LoginViewModel>(); // Реєстрація ViewModel для логування

            // Реєстрація головного вікна з залежностями
            serviceCollection.AddTransient<MainWindow>(provider =>
            {
                var loginViewModel = provider.GetRequiredService<LoginViewModel>();
                return new MainWindow(loginViewModel); 
            });

            _serviceProvider = serviceCollection.BuildServiceProvider(); // Створення постачальника служб
        }
    }
}
