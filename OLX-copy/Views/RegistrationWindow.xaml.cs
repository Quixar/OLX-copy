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


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is RegistrationViewModel vm)
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
    }
}
