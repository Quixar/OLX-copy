using OLX_copy.Data;
using OLX_copy.Services;
using OLX_copy.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OLX_copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CurrentUserService _currentUserService;
        public MainWindow(CurrentUserService currentUserService)
        {
            InitializeComponent();
            _currentUserService = currentUserService;

            DataContext = new MainViewModel(_currentUserService);
        }
        private void SearchElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!SearchResultsListBox.IsFocused)
                {
                    if (DataContext is ViewModels.MainViewModel vm)
                    {
                        vm.AreSearchResultsVisible = false;
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}