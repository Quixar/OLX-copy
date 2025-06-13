using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserPageViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public ICommand MyAdsOpenCommand { get; }
        public ICommand CustomizationCommand { get; }

        public UserPageViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            MyAdsOpenCommand = new RelayCommand(MyAdsOpen);
            CustomizationCommand = new RelayCommand(OpenCustomization);
        }

        private void OpenCustomization(object parameter)
        {
            var window = new UserCustomazationPage(_currentUserService);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.UserHomePage);

            loginWindow?.Close();
        }

        private void MyAdsOpen(object parametr)
        {
            var window = new UserMyAdsPage(_currentUserService);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.UserHomePage);

            loginWindow?.Close();
        }
    }
}
