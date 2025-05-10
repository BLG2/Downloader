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

        public MainWindowVM()
        {
            AppName = AppConfig.AppName;
            CurrentPage = DownloaderPage.GetInstance();

            #region COMMANDS
            NavigatePageCommand = new RelayCommand<RadioButton>((x) => NavigatePage(x));
            MinimizeAppCommand = new RelayCommand<MainWindow>((x) => MinimizeApplication(x));
            MaximizeAppCommand = new RelayCommand<MainWindow>((x) => MaximizeApplication(x));
            CloseAppCommand = new RelayCommand<MainWindow>((x) => CloseApplication(x));
            DragOnHoldCommand = new RelayCommand<MainWindow>((x) => DragOnHoldApplication(x));
            #endregion
        }

        #region COMMANDS
        public ICommand NavigatePageCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public ICommand MaximizeAppCommand { get; }
        public ICommand CloseAppCommand { get; }
        public ICommand DragOnHoldCommand { get; }
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

        private void MinimizeApplication(MainWindow window)
        {
            window.WindowState = window.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }

        private void MaximizeApplication(MainWindow window)
        {
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseApplication(MainWindow window)
        {
            window.Close();
        }

        private void DragOnHoldApplication(MainWindow window)
        {
            window.Opacity = 0.8;
            window?.DragMove();
            window.Opacity = 1;
        }
        #endregion
    }
}
