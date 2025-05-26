using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OLX_copy.ViewModels;

namespace OLX_copy
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void LoginTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = UsernameTextBox.Text.Trim();
                string name = NameTextBox.Text.Trim();
                string password = PasswordBox.Password.Trim();
                string email = EmailTextBox.Text.Trim();

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                if (password.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.");
                    return;
                }

                using var context = new Data.DataContext();

                if (context.UserAccesses.Any(u => u.Login == login))
                {
                    MessageBox.Show("This login is already taken.");
                    return;
                }

                Guid userId = Guid.NewGuid();
                string salt = PasswordHasher.GenerateSalt();
                string dk = PasswordHasher.HashPassword(password, salt);

                var user = new Data.Entities.User
                {
                    Id = userId,
                    Name = name,
                    Email = email,
                    RegisteredAt = DateTime.Now
                };

                var userAccess = new Data.Entities.UserAccess
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = "user",
                    Login = login,
                    Salt = salt,
                    Dk = dk
                };

                context.Users.Add(user);
                context.UserAccesses.Add(userAccess);
                context.SaveChanges();

                MessageBox.Show("Registration successful!");

                var mainMenuWindow = new MainWindow();
                mainMenuWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                    errorMessage += "\nInner Exception: " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    errorMessage += "\nDeepest: " + ex.InnerException.InnerException.Message;

                MessageBox.Show($"Ошибка при регистрации: {errorMessage}");
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
                PasswordWatermark.Visibility = Visibility.Collapsed;
            else
                PasswordWatermark.Visibility = Visibility.Visible;
        }
    }
}
