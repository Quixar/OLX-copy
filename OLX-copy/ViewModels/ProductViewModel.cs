using OLX_copy.Data.Entities;
using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly CurrentUserService _currentUserService;

        private string _selectedImageUrl;
        private int _quantity = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public ObservableCollection<string> Images { get; }

        // Главное изображение
        public string MainImageUrl
        {
            get => _selectedImageUrl ?? Images.FirstOrDefault();
            set
            {
                if (_selectedImageUrl != value)
                {
                    _selectedImageUrl = value;
                    OnPropertyChanged(nameof(MainImageUrl));
                }
            }
        }

        // Количество товара
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

            // Гарантируем, что Images не null
            Images = new ObservableCollection<string>(
                product?.Images?.Select(i => i.ImageUrl).Where(url => !string.IsNullOrEmpty(url)) ?? new List<string>()
            );

            _selectedImageUrl = Images.FirstOrDefault();
        }

        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

