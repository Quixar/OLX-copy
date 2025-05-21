using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OLX_copy
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void RegisterTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();

            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.");
                    return;
                }

                using var context = new Data.DataContext();

                var userAccess = context.UserAccesses
                    .Include(ua => ua.User)
                    .FirstOrDefault(u => u.Login == username);

                if (userAccess == null)
                {
                    MessageBox.Show("Invalid username or password.");
                    return;
                }

                bool isPasswordValid = PasswordHasher.VerifyPassword(password, userAccess.Salt, userAccess.Dk);

                if (!isPasswordValid)
                {
                    MessageBox.Show("Invalid username or password.");
                    return;
                }

                var mainMenuWindow = new MainMenuWindow();
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
                PasswordWatermark.Visibility = Visibility.Collapsed;
            else
                PasswordWatermark.Visibility = Visibility.Visible;
        }
    }
}
