using CommunityToolkit.Mvvm.Input;
using Downloader.Pages;
using Downloader.ViewModels.Observers;
using Downloader.ViewModels.Pages;
using Downloader.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Downloader.ViewModels
{
    public class MainWindowVM : MainWindowOM
    {
        private MainWindow _view = null;

        public MainWindowVM(MainWindow view)
        {
            _view = view;
            AppName = AppConfig.AppName;
            CurrentPage = DownloaderPage.GetInstance();

            #region COMMANDS
            NavigatePageCommand = new RelayCommand<RadioButton>((x) => NavigatePage(x));
            MinimizeAppCommand = new RelayCommand(MinimizeApplication);
            MaximizeAppCommand = new RelayCommand(MaximizeApplication);
            CloseAppCommand = new RelayCommand(CloseApplication);
            CloseEvent = new RelayCommand<CancelEventArgs>((x) => CloseApplicationEvent(x));
            #endregion
        }

        #region COMMANDS
        public ICommand NavigatePageCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public ICommand MaximizeAppCommand { get; }
        public ICommand CloseAppCommand { get; }
        public ICommand CloseEvent { get; }
        #endregion

        #region Command Actions 
        private void NavigatePage(RadioButton btn)
        {
            var btnName = btn.Name;

            switch (btnName.ToLower())
            {
                case "convert":
                    CurrentPage = ConvertorPage.GetInstance();
                    break;
                case "download":
                default:
                    CurrentPage = DownloaderPage.GetInstance();
                    break;
            }
        }

        private void MinimizeApplication()
        {
            _view.WindowState = _view.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }

        private void MaximizeApplication()
        {
            _view.WindowState = _view.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseApplication()
        {
            _view.Close();
        }

        public void CloseApplicationEvent(CancelEventArgs e)
        {
            if (DownloaderPageVM.DownloadItems != null && DownloaderPageVM.DownloadItems.Any(x => x.Downloading))
            {
                foreach (var download in DownloaderPageVM.DownloadItems.Where(x => x.Downloading))
                {
                    download.StopDownloading();
                }
            }
            if (ConvertorPageVM.ConvertItems != null && ConvertorPageVM.ConvertItems.Any(x => x.Converting))
            {
                foreach (var convert in ConvertorPageVM.ConvertItems.Where(x => x.Converting))
                {
                    convert.StopConverting();
                }
            }
        }
        #endregion
    }
}
