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

        public MainViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            OpenUserPageCommand = new RelayCommand(OpenUserPage);
            SearchCommand = new RelayCommand(ExecuteSearch);

            LoadLatestProducts();
        }

        private void OpenUserPage(object parameter)
        {
            var window = new UserHomePage(_currentUserService);
            window.Show();
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
                .Take(10)
                .ToList();

            LatestProducts = products;
        }

        private void ExecuteSearch(object parameter)
        {
            var query = _searchQuery?.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                LoadLatestProducts();
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
                .ToList();

            LatestProducts = results;
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}