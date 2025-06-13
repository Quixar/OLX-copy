using OLX_copy.Data;
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
    /// Логика взаимодействия для UserMyAdsPage.xaml
    /// </summary>
    public partial class UserMyAdsPage : Window
    {
        private readonly CurrentUserService _currentUserService;
        private readonly DataContext _dataContext;

        public UserMyAdsPage(CurrentUserService currentUserService)
        {
            InitializeComponent();
            _currentUserService = currentUserService;
            _dataContext = new DataContext(); // Создаем экземпляр DataContext
            DataContext = new UserMyAdsViewModel(_currentUserService, _dataContext);
        }

        protected override void OnClosed(EventArgs e)
        {
            _dataContext.Dispose(); // Важно освободить ресурсы
            base.OnClosed(e);
        }
    }
}
