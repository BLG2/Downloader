using Downloader.ViewModels.Pages;
using System.Windows.Controls;

namespace Downloader.Pages
{
    /// <summary>
    /// Interaction logic for DownloaderPage.xaml
    /// </summary>
    public partial class DownloaderPage : Page
    {
        private static DownloaderPage? _instance;
        public DownloaderPageVM ViewModel { get; }
        private DownloaderPage()
        {
            ViewModel = new DownloaderPageVM(this);
            DataContext = ViewModel;
            InitializeComponent();
        }

        public static DownloaderPage GetInstance()
        {
            if (_instance == null) _instance = new DownloaderPage();
            return _instance;
        }
    }
}
