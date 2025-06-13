using OLX_copy.Data;
using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserCustomizationViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public UserCustomizationViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            OpenMyAdsCommand = new RelayCommand(MyAdsOpen);
        }

        public ICommand OpenMyAdsCommand { get; }

        private void MyAdsOpen(object parametr)
        {
            var window = new UserMyAdsPage(_currentUserService);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.UserCustomazationPage);

            loginWindow?.Close();
        }
    }
}
