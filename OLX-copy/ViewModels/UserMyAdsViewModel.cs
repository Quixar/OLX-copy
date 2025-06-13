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
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserMyAdsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DataContext _context;
        private readonly CurrentUserService _currentUserService;

        public UserMyAdsViewModel(CurrentUserService currentUserService, DataContext context)
        {
            _context = context;
            _currentUserService = currentUserService;

            DeleteProductCommand = new RelayCommand(DeleteProduct);
            OpenProductManagmentCommand = new RelayCommand(OpenProductManagment);
            OpenCustomizationCommand = new RelayCommand(OpenCustomization);

            LoadProducts();
        }

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        public ICommand DeleteProductCommand { get; }
        public ICommand OpenProductManagmentCommand { get; }
        public ICommand OpenCustomizationCommand { get; }

        private void OpenCustomization(object obj)
        {
            var window = new UserCustomazationPage(_currentUserService, _context);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.UserMyAdsPage);

            loginWindow?.Close();
        }

        private void OpenProductManagment(object obj)
        {
            var window = new ProductManagementWindow(_currentUserService);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is OLX_copy.Views.UserMyAdsPage);

            loginWindow?.Close();
        }

        private void LoadProducts()
        {
            var userId = _currentUserService.CurrentUser.Id;
            Products = _context.Products
                .Include(p => p.Images)
                .Where(p => p.UserId == userId)
                .ToList();
        }

        private void DeleteProduct(object obj)
        {
            if (obj is Product productToDelete)
            {
                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
                LoadProducts();
            }
        }


        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
