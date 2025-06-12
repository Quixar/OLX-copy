using Microsoft.EntityFrameworkCore;
using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Helpers;
using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserMyAdsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DataContext _context;
        private readonly CurrentUserService _currentUserService;

        public UserMyAdsViewModel(CurrentUserService currentUserService)
        {
            _context = new DataContext();
            _currentUserService = currentUserService;

            DeleteProductCommand = new RelayCommand(DeleteProduct);
            OpenProductManagmentCommand = new RelayCommand(OpenProductManagment);

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

        private void OpenProductManagment(object obj)
        {
            var window = new ProductManagementWindow(_currentUserService);
            window.Show();
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
