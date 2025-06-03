using OLX_copy.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using OLX_copy.Services;

namespace OLX_copy.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CurrentUserService _currentUserService;

        private string _username;
        private string _password;
        private string _email;
        private string _name;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); OnPropertyChanged(nameof(IsUsernameEmpty)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); OnPropertyChanged(nameof(IsEmailEmpty)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(IsNameEmpty)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); OnPropertyChanged(nameof(IsPasswordEmpty)); }
        }


        public bool IsUsernameEmpty => string.IsNullOrEmpty(Username);
        public bool IsPasswordEmpty => string.IsNullOrEmpty(Password);
        public bool IsEmailEmpty => string.IsNullOrEmpty(Email);
        public bool IsNameEmpty => string.IsNullOrEmpty(Name);

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                if (!IsValidEmail(Email))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                if (Password.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.");
                    return;
                }

                using var context = new Data.DataContext();

                if (context.UserAccesses.Any(u => u.Login == Username))
                {
                    MessageBox.Show("This login is already taken.");
                    return;
                }

                Guid userId = Guid.NewGuid();
                string salt = PasswordHasher.GenerateSalt();
                string dk = PasswordHasher.HashPassword(Password, salt);

                var user = new Data.Entities.User
                {
                    Id = userId,
                    Name = Name,
                    Email = Email,
                    RegisteredAt = DateTime.Now
                };

                var userAccess = new Data.Entities.UserAccess
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = "user",
                    Login = Username,
                    Salt = salt,
                    Dk = dk
                };

                context.Users.Add(user);
                context.UserAccesses.Add(userAccess);
                context.SaveChanges();

                MessageBox.Show("Registration successful!");

                _currentUserService.CurrentUser = user;
                _currentUserService.CurrentUserAccess = userAccess;

                var mainWindow = new MainWindow(_currentUserService);
                mainWindow.Show();

                var loginWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is OLX_copy.RegistrationWindow);

                loginWindow?.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                    errorMessage += "\nInner Exception: " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    errorMessage += "\nDeepest: " + ex.InnerException.InnerException.Message;

                MessageBox.Show($"Error on registration: {errorMessage}");
            }
        }


        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
