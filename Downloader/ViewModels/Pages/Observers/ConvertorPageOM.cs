using CommunityToolkit.Mvvm.ComponentModel;

namespace Downloader.ViewModels.Pages.Observers
{
    public class ConvertorPageOM : ObservableObject
    {
        private bool ConvertOnSelectVal;
        public bool ConvertOnSelect
        {
            get { return ConvertOnSelectVal; }
            set => SetProperty(ref ConvertOnSelectVal, value);
        }
        private bool IncludeAudioVal = true;
        public bool IncludeAudio
        {
            get { return IncludeAudioVal; }
            set => SetProperty(ref IncludeAudioVal, value);
        }

    }
}
