using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly CurrentUserService _currentUserService;
        private readonly Product _product;
        private readonly DataContext _context;

        private string _selectedImageUrl;
        private int _quantity = 1;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public List<string> Images { get; private set; }
        public List<int> AvailableQuantities { get; private set; }

        public string ButtonText => _product.Stock > 0 ? "Купить" : "Sold Out";
        public bool CanBuy => _product.Stock > 0;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value && value > 0)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public string MainImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_selectedImageUrl))
                    return GetAbsoluteImagePath(_selectedImageUrl);
                if (Images.Any())
                    return GetAbsoluteImagePath(Images.First());
                return string.Empty;
            }
            set
            {
                if (_selectedImageUrl != value)
                {
                    _selectedImageUrl = value;
                    OnPropertyChanged(nameof(MainImageUrl));
                }
            }
        }

        // Команды
        public ICommand BuyCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand OpenMainCommand { get; }
        public ICommand OpenHomePageCommand { get; }

        public ProductViewModel(CurrentUserService currentUserService, Product product)
        {
            _currentUserService = currentUserService;
            _product = product;
            _context = new DataContext();

            BuyCommand = new RelayCommand(BuyProduct);
            SelectImageCommand = new RelayCommand(obj => { if (obj is string url) MainImageUrl = url; });
            OpenMainCommand = new RelayCommand(OpenMain);
            OpenHomePageCommand = new RelayCommand(OpenUserPage);

            InitializeProductData();
            InitializeImages();
            InitializeQuantities();
        }

        // --- Инициализация свойств ---
        private void InitializeProductData()
        {
            Name = _product?.Name ?? "Unnamed";
            Price = _product?.Price ?? 0m;
            Description = _product?.Description ?? "No description";
        }

        private void InitializeImages()
        {
            Images = _product.Images?
                .Select(ii => ii.ImageUrl)
                .Where(url => !string.IsNullOrEmpty(url))
                .ToList() ?? new List<string>();

            _selectedImageUrl = Images.FirstOrDefault() ?? string.Empty;
        }

        private void InitializeQuantities()
        {
            AvailableQuantities = Enumerable.Range(1, Math.Min(_product.Stock, 10)).ToList();
        }

        // --- Команды ---
        private void BuyProduct(object obj)
        {
            if (_product.Stock >= _quantity)
            {
                _product.Stock -= _quantity;
                _context.Products.Update(_product);
                _context.SaveChanges();

                MessageBox.Show($"Вы купили {_quantity} шт. товара \"{_product.Name}\".", "Покупка", MessageBoxButton.OK, MessageBoxImage.Information);

                OnPropertyChanged(nameof(ButtonText));
                OnPropertyChanged(nameof(CanBuy));
                InitializeQuantities();
                OnPropertyChanged(nameof(AvailableQuantities));
            }
            else
            {
                MessageBox.Show("Недостаточно товара на складе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenMain(object obj)
        {
            var mainWindow = new MainWindow(_currentUserService);
            mainWindow.Show();
            CloseProductWindow();
        }

        public void OpenUserPage(object obj)
        {
            var userPage = new UserHomePage(_currentUserService);
            userPage.Show();
            CloseProductWindow();
        }

        // --- Вспомогательные методы ---
        private void CloseProductWindow()
        {
            var productWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.ProductWindow);
            productWindow?.Close();
        }

        private string GetAbsoluteImagePath(string relativePath)
        {
            string absolutePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", relativePath));
            return new Uri(absolutePath).AbsoluteUri;
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
