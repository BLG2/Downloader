using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace Downloader.ViewModels.Observers
{
    public class MainWindowOM : ObservableObject
    {
        private Page CurrentPageVal;
        public Page CurrentPage
        {
            get => CurrentPageVal;
            set
            {
                CurrentPageVal = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        private string AppNameVal;
        public string AppName
        {
            get { return AppNameVal; }
            set => SetProperty(ref AppNameVal, value);
        }

    }
}
