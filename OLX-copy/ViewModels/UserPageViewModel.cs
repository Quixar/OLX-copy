using OLX_copy.Helpers;
using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserPageViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public ICommand MyAdsOpenCommand { get; }

        public UserPageViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            MyAdsOpenCommand = new RelayCommand(MyAdsOpen);
        }

        private void MyAdsOpen(object parametr)
        {
            var window = new ProductManagementWindow(_currentUserService);
            window.Show();
        }
    }
}
