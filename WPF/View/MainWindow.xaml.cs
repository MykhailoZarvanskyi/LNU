using System.Windows;
using System.Windows.Controls;
using WpfApp2.ViewModels;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        // Конструктор, який приймає LoginViewModel
        public MainWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel; // Зберігаємо ViewModel
            DataContext = _viewModel; // Встановлюємо DataContext
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                // Оновлюємо пароль у ViewModel
                _viewModel.Password = passwordBox.Password;
            }
        }
    }
}
