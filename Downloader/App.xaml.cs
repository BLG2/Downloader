using Downloader.ViewModels;
using System.Windows;
using Downloader.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Downloader.ViewModels.Pages;

namespace Downloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
             .ConfigureServices((context, services) =>
             {
                 services.AddSingleton<MainWindow>();
                 services.AddSingleton<MainWindowVM>();
             })
             .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            SetupExceptionHandling();

            #region RUNN APP ONLY 1 TIME ON DESKTOP
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location)).Count() > 1)
            {
                MessageBox.Show("Another instance of the tool is already running.", "Alert");
                Current.Shutdown();
            }
            #endregion

            await AppHost.StartAsync();

            MainWindow mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
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

            await AppHost.StopAsync();
            base.OnExit(e);
        }

        #region Error Logging
        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                ReportException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            };

            DispatcherUnhandledException += (_, e) =>
            {
                ReportException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                ReportException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }
        private static void ReportException(Exception exception, string source)
        {
            MessageBox.Show(
                $"An unexpected error occured.\n\nSource:{source}\nException:{exception.Message}\nException Source:{exception.Source}",
                "Error"
            );
        }
        #endregion

    }
}
