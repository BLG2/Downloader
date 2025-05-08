using Downloader.ViewModels;
using System.Windows;
using Downloader.Views;

namespace Downloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region Show Main view and bind custom datacontext
            MainWindow View = new MainWindow();
            MainWindowVM vm = new MainWindowVM(View);
            View.DataContext = vm;
            View.Show();
            #endregion
        }
    }

}
