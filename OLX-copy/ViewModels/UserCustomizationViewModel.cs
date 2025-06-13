using OLX_copy.Data;
using OLX_copy.Data.Entities;
using OLX_copy.Helpers;
using OLX_copy.Services;
using OLX_copy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OLX_copy.ViewModels
{
    public class UserCustomizationViewModel : INotifyPropertyChanged
    {
        private readonly CurrentUserService _currentUserService;
        private readonly DataContext _dataContext;
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand OpenMyAdsCommand { get; }

        public UserCustomizationViewModel(CurrentUserService currentUserService, DataContext dataContext)
        {
            _currentUserService = currentUserService;
            _dataContext = dataContext;
            CurrentUser = _currentUserService.CurrentUser; // Загружаем текущего пользователя

            SaveCommand = new RelayCommand(SaveChanges);
            OpenMyAdsCommand = new RelayCommand(MyAdsOpen);
        }

        private void SaveChanges(object parameter)
        {
            if (string.IsNullOrEmpty(CurrentUser.Name))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }

            try
            {
                _dataContext.Users.Update(CurrentUser);
                _dataContext.SaveChanges();
                MessageBox.Show("Changes saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void MyAdsOpen(object parameter)
        {
            var window = new UserMyAdsPage(_currentUserService);
            window.Show();

            var loginWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is UserCustomazationPage);
            loginWindow?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
