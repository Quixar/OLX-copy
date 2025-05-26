using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OLX_copy.ViewModels;

namespace OLX_copy
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is LoginViewModel vm)
            {
                vm.Password = passwordBox.Password;
            }

            if (FindName("PasswordWatermark") is TextBlock passwordWatermark)
            {
                passwordWatermark.Visibility = string.IsNullOrEmpty(((PasswordBox)sender).Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void RegisterTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
