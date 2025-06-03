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
    public class MainViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public ICommand OpenProductManagerCommand { get; }
        public MainViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            OpenProductManagerCommand = new RelayCommand(OpenProductManager);
        }

        private void OpenProductManager(object parameter)
        {
            var window = new ProductManagementWindow();
            window.Show();
        }
    }
}
