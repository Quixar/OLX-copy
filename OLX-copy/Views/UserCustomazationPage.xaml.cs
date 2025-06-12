using OLX_copy.Services;
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
    /// Логика взаимодействия для UserCustomazationPage.xaml
    /// </summary>
    public partial class UserCustomazationPage : Window
    {
        private readonly CurrentUserService _currentUserService;
        public UserCustomazationPage(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            InitializeComponent();
        }
    }
}
