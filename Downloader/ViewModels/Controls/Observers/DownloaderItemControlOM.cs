using CommunityToolkit.Mvvm.ComponentModel;
using Downloader.Enums;
using Downloader.Models;

namespace Downloader.ViewModels.Controls.Observers
{
    public class DownloaderItemControlOM : ObservableObject
    {
        private YoutubeVideoModel VideoVal;
        public YoutubeVideoModel Video
        {
            get { return VideoVal; }
            set => SetProperty(ref VideoVal, value);
        }

        private string DownloadedPathVal;
        public string DownloadedPath
        {
            get { return DownloadedPathVal; }
            set => SetProperty(ref DownloadedPathVal, value);
        }

        private string ProgressTextVal;
        public string ProgressText
        {
            get { return ProgressTextVal; }
            set => SetProperty(ref ProgressTextVal, value);
        }

        private bool CompletedVal;
        public bool Completed
        {
            get { return CompletedVal; }
            set => SetProperty(ref CompletedVal, value);
        }

        private bool DownloadingVal;
        public bool Downloading
        {
            get { return DownloadingVal; }
            set => SetProperty(ref DownloadingVal, value);
        }

        private bool IncludeAudioVal = true;
        public bool IncludeAudio
        {
            get { return IncludeAudioVal; }
            set => SetProperty(ref IncludeAudioVal, value);
        }

        private IEnumerable<FileFormatsEnum> AvailebleFormatsVal;
        public IEnumerable<FileFormatsEnum> AvailebleFormats
        {
            get => AvailebleFormatsVal;
            set => SetProperty(ref AvailebleFormatsVal, value);
        }

        private FileFormatsEnum SelectedFormatVal;
        public FileFormatsEnum SelectedFormat
        {
            get => SelectedFormatVal;
            set => SetProperty(ref SelectedFormatVal, value);
        }

    }
}
