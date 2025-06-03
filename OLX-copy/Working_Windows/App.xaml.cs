using OLX_copy.Services;
using OLX_copy.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OLX_copy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly CurrentUserService _currentUserService = new CurrentUserService();

        [STAThread]
        public static void Main()
        {
            var app = new App();
            var window = new LoginWindow(_currentUserService); // или MainWindow
            app.Run(window);
        }
    }

}
