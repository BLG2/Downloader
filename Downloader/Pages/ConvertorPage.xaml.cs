using Downloader.ViewModels.Pages;
using System.Windows.Controls;

namespace Downloader.Pages
{
    /// <summary>
    /// Interaction logic for ConvertorPage.xaml
    /// </summary>
    public partial class ConvertorPage : Page
    {

        private static ConvertorPage? _instance;
        private ConvertorPageVM ViewModel { get; }
        private ConvertorPage()
        {
            ViewModel = new ConvertorPageVM();
            DataContext = ViewModel;
            InitializeComponent();
        }

        public static ConvertorPage GetInstance()
        {
            if (_instance == null) _instance = new ConvertorPage();
            return _instance;
        }
    }
}
