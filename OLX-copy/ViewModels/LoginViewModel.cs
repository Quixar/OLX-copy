using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using OLX_copy.Data;
using OLX_copy.Helpers;
using Microsoft.EntityFrameworkCore;
using OLX_copy.Data.Entities;
using OLX_copy.Services;

namespace OLX_copy.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CurrentUserService _currentUserService;

        private string _username;
        private string _password;

        public List<Product> Products { get; set; } = new();

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); OnPropertyChanged(nameof(IsUsernameEmpty)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); OnPropertyChanged(nameof(IsPasswordEmpty)); }
        }

        public bool IsUsernameEmpty => string.IsNullOrEmpty(Username);
        public bool IsPasswordEmpty => string.IsNullOrEmpty(Password);

        public ICommand LoginCommand { get; }

        public LoginViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object obj)
        {
            try
            {
                using var context = new DataContext();
                var userAccess = context.UserAccesses
                    .Include(u => u.User)
                    .Include(u => u.UserRole)
                    .FirstOrDefault(u => u.Login == Username);

                if (userAccess == null || !PasswordHasher.VerifyPassword(Password, userAccess.Salt, userAccess.Dk))
                {
                    MessageBox.Show("Invalid username or password.");
                    return;
                }

                _currentUserService.CurrentUser = userAccess.User;
                _currentUserService.CurrentUserAccess = userAccess;

                var mainWindow = new MainWindow(_currentUserService);
                mainWindow.Show();

                var loginWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is OLX_copy.LoginWindow);

                loginWindow?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message);
            }
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
