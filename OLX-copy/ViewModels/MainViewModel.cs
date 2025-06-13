using Microsoft.EntityFrameworkCore;
using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly CurrentUserService _currentUserService;
        private readonly DataContext _context = new();

        public ICommand OpenUserPageCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand UpdateWindowCommand { get; }

        public MainViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            OpenUserPageCommand = new RelayCommand(OpenUserPage);
            SearchCommand = new RelayCommand(ExecuteSearch);
            FilterByCategoryCommand = new RelayCommand(FilterByCategory);

            UpdateWindowCommand = new RelayCommand(UpdateWindow);
            LoadLatestProducts();
            LoadRandomProducts();
            LoadProductGroups();
        }

        public void UpdateWindow(object obj)
        {
            LoadLatestProducts();
            LoadRandomProducts();
            LoadProductGroups();
        }

        private void FilterByCategory(object parameter)
        {
            if (parameter is Guid groupId)
            {
                var filteredProducts = _context.Products
                    .Include(p => p.Images)
                    .Include(p => p.ProductGroup)
                    .Where(p => p.GroupId == groupId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList();

                RandomProducts = filteredProducts;
            }
        }

        private void OpenUserPage(object parameter)
        {
            var window = new UserHomePage(_currentUserService);
            window.Show();

            var mainWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.MainWindow);

            mainWindow?.Close();
        }

        private bool _areSearchResultsVisible;

        public bool AreSearchResultsVisible
        {
            get => _areSearchResultsVisible;
            set
            {
                if (_areSearchResultsVisible != value)
                {
                    _areSearchResultsVisible = value;
                    OnPropertyChanged(nameof(AreSearchResultsVisible));
                }
            }
        }

        private List<Product> _searchResults;
        public List<Product> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
                OnPropertyChanged(nameof(AreSearchResultsVisible));
            }
        }

        private List<Product> _latestProducts;
        public List<Product> LatestProducts
        {
            get => _latestProducts;
            set
            {
                _latestProducts = value;
                OnPropertyChanged(nameof(LatestProducts));
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private void LoadLatestProducts()
        {
            var products = _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductGroup)
                .OrderByDescending(p => p.CreatedAt)
                .Take(3)
                .ToList();

            LatestProducts = products;
        }

        private void ExecuteSearch(object parameter)
        {
            var query = _searchQuery?.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                SearchResults = null;
                return;
            }

            var results = _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductGroup)
                .Where(p =>
                    p.Name.ToLower().Contains(query) ||
                    p.Description.ToLower().Contains(query) ||
                    p.ProductGroup.Name.ToLower().Contains(query))
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToList();

            SearchResults = results;
            AreSearchResultsVisible = SearchResults.Any();
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private List<Product> _randomProducts;
        public List<Product> RandomProducts
        {
            get => _randomProducts;
            set
            {
                _randomProducts = value;
                OnPropertyChanged(nameof(RandomProducts));
            }
        }

        private void LoadRandomProducts()
        {
            var random = new Random();

            var products = _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductGroup)
                .Where(p => p.DeletedAt == null && p.Stock > 0)
                .AsEnumerable()
                .OrderBy(p => random.Next())
                .Take(8)
                .ToList();

            RandomProducts = products;
        }

        private List<ProductGroup> _productGroups;
        public List<ProductGroup> ProductGroups
        {
            get => _productGroups;
            set
            {
                _productGroups = value;
                OnPropertyChanged(nameof(ProductGroups));
            }
        }

        private void LoadProductGroups()
        {
            ProductGroups = _context.ProductGroups
                .OrderBy(g => g.Name)
                .ToList();
        }
    }
}