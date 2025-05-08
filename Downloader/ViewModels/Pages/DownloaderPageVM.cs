using CommunityToolkit.Mvvm.Input;
using Downloader.Helpers;
using Downloader.Models;
using Downloader.Pages;
using Downloader.ViewModels.Controls;
using Downloader.ViewModels.Pages.Observers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Downloader.ViewModels.Pages
{
    public class DownloaderPageVM : DownloaderPageOM
    {
        private DownloaderPage _view = null;
        public static ObservableCollection<DownloaderItemControlVM> DownloadItems { get; set; } = new();

        public DownloaderPageVM(DownloaderPage view)
        {
            _view = view;

            #region COMMANDS
            AddUrlCommand = new RelayCommand(AddUrl);
            #endregion
        }

        #region COMMANDS
        public ICommand AddUrlCommand { get; }
        #endregion

        #region Command Actions 
        public async void AddUrl()
        {
            try
            {
                if (string.IsNullOrEmpty(Url) || string.IsNullOrWhiteSpace(Url))
                {
                    MessageBox.Show("No url found.", "Error while adding url");
                    return;
                }

                YoutubeVideoModel? video = await YoutubeVideoHelper.GetYoutubeVidInfo(Url);
                if (video == null)
                {
                    MessageBox.Show("Unable getting video info.", "Error while adding url");
                    return;
                }

                DownloadItems.Add(new DownloaderItemControlVM(video, DownwloadOnAdd, IncludeAudio));
                Url = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while adding url");
            }
        }
        #endregion
    }
}
