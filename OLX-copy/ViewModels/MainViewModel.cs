using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class MainViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public ICommand OpenUserPageCommand { get; }
        public MainViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            OpenUserPageCommand = new RelayCommand(OpenUserPage);
        }

        private void OpenUserPage(object parameter)
        {
            var window = new UserHomePage(_currentUserService);
            window.Show();
        }
    }
}
