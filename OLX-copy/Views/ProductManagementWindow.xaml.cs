using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Working_Windows;
using System.Linq;
using System.Windows;

namespace OLX_copy
{
    public partial class ProductManagementWindow : Window
    {
        private readonly DataContext _context = new();
        private readonly Guid _userId; // сюди треба передавати ID користувача

        public ProductManagementWindow()
        {
            InitializeComponent();
        }

        public ProductManagementWindow(Guid userId)
        {
            InitializeComponent();
            _userId = userId;
            DataContext = new OLX_copy.ViewModels.ProductManagementViewModel(_userId);
        }

    }
}