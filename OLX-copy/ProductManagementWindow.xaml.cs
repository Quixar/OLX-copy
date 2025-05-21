using OLX_copy.Data;
using OLX_copy.Data.Entities;
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
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = _context.Products
                .Where(p => p.UserId == _userId)
                .ToList();
            ProductList.ItemsSource = products;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Нове оголошення",
                Price = 100,
                Stock = 1,
                GroupId = _context.ProductGroups.First().Id, // для прикладу
                UserId = _userId
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            LoadProducts();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProductList.SelectedItem as Product;
            if (selected != null)
            {
                _context.Products.Remove(selected);
                _context.SaveChanges();
                LoadProducts();
            }
        }
    }
}