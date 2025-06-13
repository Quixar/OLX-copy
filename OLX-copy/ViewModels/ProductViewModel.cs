using OLX_copy.Data.Entities;
using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly CurrentUserService _currentUserService;

        private string _selectedImageUrl;
        private int _quantity = 1;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public List<string> Images { get; }
        public ICommand BuyCommand { get; }

        public string MainImageUrl
        {
            get => _selectedImageUrl ?? Images.FirstOrDefault() ?? string.Empty;
            set
            {
                if (_selectedImageUrl != value)
                {
                    _selectedImageUrl = value;
                    OnPropertyChanged(nameof(MainImageUrl));
                }
            }
        }

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

        public ProductViewModel(CurrentUserService currentUserService, Product product)
        {
            _currentUserService = currentUserService;

            Name = product?.Name ?? "Unnamed";
            Price = product?.Price ?? 0m;
            Description = product?.Description ?? "No description";

            Images = product?.Images
                .Select(img => img.ImageUrl)
                .Where(url => !string.IsNullOrEmpty(url))
                .ToList() ?? new List<string>();

            _selectedImageUrl = Images.FirstOrDefault() ?? string.Empty;
        }

        private void BuyProduct(object obj)
        {
        }

        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

