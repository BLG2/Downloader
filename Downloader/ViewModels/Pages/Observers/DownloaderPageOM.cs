using CommunityToolkit.Mvvm.ComponentModel;

namespace Downloader.ViewModels.Pages.Observers
{
    public class DownloaderPageOM : ObservableObject
    {
        private bool DownwloadOnAddVal;
        public bool DownwloadOnAdd
        {
            get { return DownwloadOnAddVal; }
            set => SetProperty(ref DownwloadOnAddVal, value);
        }
        private string UrlVal;
        public string Url
        {
            get { return UrlVal; }
            set => SetProperty(ref UrlVal, value);
        }

        private bool IncludeAudioVal = true;
        public bool IncludeAudio
        {
            get { return IncludeAudioVal; }
            set => SetProperty(ref IncludeAudioVal, value);
        }

    }
}
