using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Services;
using OLX_copy.ViewModels;
using System.Linq;
using System.Windows;

namespace OLX_copy
{
    public partial class ProductManagementWindow : Window
    {
        private readonly CurrentUserService _currentUserService;

        public ProductManagementWindow(CurrentUserService currentUserService)
        {
            InitializeComponent();
            _currentUserService = currentUserService;
            DataContext = new ProductManagementViewModel(currentUserService);
        }

    }
}