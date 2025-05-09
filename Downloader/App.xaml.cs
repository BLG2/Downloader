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

            #region RUNN APP ONLY 1 TIME ON DESKTOP
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location)).Count() > 1)
            {
                MessageBox.Show("You can only runn one instance of this application.", "Alert");
                Application.Current.Shutdown();
            }
            #endregion

            #region Show Main view and bind custom datacontext
            MainWindow View = new MainWindow();
            MainWindowVM vm = new MainWindowVM(View);
            View.DataContext = vm;
            View.Show();
            #endregion
        }
    }

}
