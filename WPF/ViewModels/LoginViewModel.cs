using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using WpfApp2.Services;
using DTO; 
using System.Threading.Tasks;
using WpfApp2.View;

namespace WpfApp2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Username and Password cannot be empty.");
                return;
            }

            try
            {
                var userRole = await _authenticationService.AuthenticateAsync(Username, Password);
                if (userRole != null)
                {
                    MessageBox.Show("Login successful!");

                    // Відкриваємо нове вікно
                    var dashboardWindow = new DashboardWindow();
                    dashboardWindow.Show();

                    
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
