using OLX_copy.Data.Entities;
using OLX_copy.Data;
using OLX_copy.Helpers;
using OLX_copy.Services;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;

public class ProductManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly DataContext _context;
    private readonly CurrentUserService _currentUserService;

    public ProductManagementViewModel(CurrentUserService currentUserService)
    {
        _context = new DataContext();
        _currentUserService = currentUserService;

        AddProductCommand = new RelayCommand(AddProduct);
        ChooseImagesCommand = new RelayCommand(ChooseImages);

        SelectedImagePaths = new List<string>();
        DisplayedImageNames = new List<string>();
        LoadGroups();
    }

    // Свойства для нового продукта
    public string NewProductName { get => _newProductName; set { _newProductName = value; OnPropertyChanged(nameof(NewProductName)); } }
    private string _newProductName;

    public string NewProductDescription { get => _newProductDescription; set { _newProductDescription = value; OnPropertyChanged(nameof(NewProductDescription)); } }
    private string _newProductDescription;

    public string NewProductPrice { get => _newProductPrice; set { _newProductPrice = value; OnPropertyChanged(nameof(NewProductPrice)); } }
    private string _newProductPrice;

    private ProductGroup _selectedGroup;
    public ProductGroup SelectedGroup
    {
        get => _selectedGroup;
        set { _selectedGroup = value; OnPropertyChanged(nameof(SelectedGroup)); }
    }

    private List<ProductGroup> _productGroups;
    public List<ProductGroup> ProductGroups
    {
        get => _productGroups;
        set { _productGroups = value; OnPropertyChanged(nameof(ProductGroups)); }
    }

    public List<string> SelectedImagePaths { get; set; }
    private List<string> _displayedImageNames;
    public List<string> DisplayedImageNames
    {
        get => _displayedImageNames;
        set { _displayedImageNames = value; OnPropertyChanged(nameof(DisplayedImageNames)); }
    }

    public ICommand AddProductCommand { get; }
    private void LoadGroups()
    {
        ProductGroups = _context.ProductGroups.ToList();
    }

    public ICommand ChooseImagesCommand { get; }
    private void ChooseImages(object obj)
    {
        OpenFileDialog dialog = new()
        {
            Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Multiselect = true
        };

        if (dialog.ShowDialog() == true)
        {
            SelectedImagePaths.Clear();
            DisplayedImageNames.Clear();

            foreach (var file in dialog.FileNames)
            {
                SelectedImagePaths.Add(file);
                DisplayedImageNames.Add(Path.GetFileName(file));
            }

            OnPropertyChanged(nameof(DisplayedImageNames));
        }
    }

    private void AddProduct(object obj)
    {
        if (!decimal.TryParse(NewProductPrice, out decimal price))
        {
            MessageBox.Show("Некоректна ціна.");
            return;
        }

        if (SelectedGroup == null)
        {
            MessageBox.Show("Виберіть групу.");
            return;
        }

        if (string.IsNullOrWhiteSpace(NewProductName))
        {
            MessageBox.Show("Введіть назву продукту.");
            return;
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = NewProductName,
            Description = NewProductDescription,
            GroupId = SelectedGroup.Id,
            Price = price,
            UserId = _currentUserService.CurrentUser.Id,
            Slug = Guid.NewGuid().ToString(),
            Stock = 1
        };

        _context.Products.Add(product);

        // Путь к корню проекта (выход из bin/Debug/...)
        string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string imageFolder = Path.Combine(projectRoot, "Views", "images");

        Directory.CreateDirectory(imageFolder);

        for (int i = 0; i < SelectedImagePaths.Count; i++)
        {
            var path = SelectedImagePaths[i];
            var fileName = $"{product.Id}_{i}{Path.GetExtension(path)}";
            var savePath = Path.Combine(imageFolder, fileName);

            try
            {
                File.Copy(path, savePath, overwrite: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при копіюванні зображення: {ex.Message}");
                return;
            }

            string relativePath = Path.Combine("Views", "images", fileName);

            _context.ItemImages.Add(new ItemImage
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                ImageUrl = relativePath,
                Order = i
            });
        }

        try
        {
            _context.SaveChanges();
            MessageBox.Show("Оголошення створено.");
        }
        catch (Exception ex)
        {
            StringBuilder sb = new();
            while (ex != null)
            {
                sb.AppendLine(ex.Message);
                ex = ex.InnerException;
            }

            MessageBox.Show($"Помилка при збереженні даних:\n{sb}");
        }
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
