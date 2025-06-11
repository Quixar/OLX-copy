using OLX_copy.Services;
using OLX_copy.ViewModels;
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

namespace OLX_copy.Views
{
    /// <summary>
    /// Логика взаимодействия для UserHomePage.xaml
    /// </summary>
    public partial class UserHomePage : Window
    {
        private readonly CurrentUserService _currentUserService;
        public UserHomePage(CurrentUserService currentUserService)
        {
            InitializeComponent();
            _currentUserService = currentUserService;
            DataContext = new UserPageViewModel(_currentUserService);
        }
    }
}
