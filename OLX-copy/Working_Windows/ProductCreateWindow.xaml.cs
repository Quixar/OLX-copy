using Microsoft.Win32;
using OLX_copy.Data.Entities;
using OLX_copy.Data;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace OLX_copy.Working_Windows
{
    /// <summary>
    /// Interaction logic for ProductCreateWindow.xaml
    /// </summary>
    public partial class ProductCreateWindow : Window
    {
        private readonly DataContext _context = new();
        private readonly Guid _userId;
        private readonly List<string> _selectedImagePaths = new();

        public ProductCreateWindow(Guid userId)
        {
            InitializeComponent();
            _userId = userId;

            GroupBox.ItemsSource = _context.ProductGroups.ToList();
            GroupBox.DisplayMemberPath = "Name";
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    _selectedImagePaths.Add(file);
                    ImagesList.Items.Add(System.IO.Path.GetFileName(file));
                }
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Некоректна ціна.");
                return;
            }

            var group = GroupBox.SelectedItem as ProductGroup;
            if (group == null)
            {
                MessageBox.Show("Виберіть групу.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введіть назву продукту.");
                return;
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = NameBox.Text,
                Description = DescriptionBox.Text,
                GroupId = group.Id,
                Price = price,
                UserId = _userId,
                Slug = Guid.NewGuid().ToString(),
                Stock = 1
            };

            _context.Products.Add(product);

            int index = 0;
            foreach (var path in _selectedImagePaths)
            {
                var fileName = $"{product.Id}_{index}{System.IO.Path.GetExtension(path)}";
                var savePath = System.IO.Path.Combine("images", fileName);

                try
                {
                    Directory.CreateDirectory("images");
                    File.Copy(path, savePath, overwrite: true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при копіюванні файлу: {ex.Message}");
                    return;
                }

                var image = new ItemImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = savePath,
                    Order = index
                };

                _context.ItemImages.Add(image);
                index++;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new();
                Exception currentEx = ex;
                while (currentEx != null)
                {
                    sb.AppendLine(currentEx.Message);
                    currentEx = currentEx.InnerException;
                }
                MessageBox.Show($"Помилка при збереженні даних:\n{sb}");
                return;
            }

            MessageBox.Show("Оголошення створено.");
            Close();
        }

    }
}
