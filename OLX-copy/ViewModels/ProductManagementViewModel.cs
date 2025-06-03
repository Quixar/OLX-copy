using Microsoft.EntityFrameworkCore;
using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Working_Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class ProductManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DataContext _context;
        private readonly CurrentUserService _currentUserService;

        private List<Product> _products;
        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductsCommand { get; }

        public ProductManagementViewModel(CurrentUserService currentUserService)
        {
            _context = new DataContext();
            _currentUserService = currentUserService;

            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductsCommand = new RelayCommand(DeleteProduct);

            LoadProducts();
        }

        private void AddProduct(object obj)
        {
            var createWindow = new ProductCreateWindow(_currentUserService.CurrentUser.Id);
            createWindow.ShowDialog();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var userId = _currentUserService.CurrentUser.Id;

            Products = _context.Products
                .Where(p => p.UserId == userId)
                .ToList();
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct != null)
            {
                _context.Products.Remove(SelectedProduct);
                _context.SaveChanges();
                LoadProducts();
            }
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
