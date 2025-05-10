using CommunityToolkit.Mvvm.ComponentModel;
using Downloader.Enums;

namespace Downloader.ViewModels.Controls.Observers
{
    public class ConvertorFileControlOM : ObservableObject
    {
        private string FileNameVal;
        public string FileName
        {
            get { return FileNameVal; }
            set => SetProperty(ref FileNameVal, value);
        }

        private string FilePathVal;
        public string FilePath
        {
            get { return FilePathVal; }
            set => SetProperty(ref FilePathVal, value);
        }

        private string ConvertedFilePathVal;
        public string ConvertedFilePath
        {
            get { return ConvertedFilePathVal; }
            set => SetProperty(ref ConvertedFilePathVal, value);
        }

        private bool CompletedVal;
        public bool Completed
        {
            get { return CompletedVal; }
            set => SetProperty(ref CompletedVal, value);
        }

        private bool ConvertingVal;
        public bool Converting
        {
            get { return ConvertingVal; }
            set => SetProperty(ref ConvertingVal, value);
        }

        private bool ProgressIndicatingVal;
        public bool ProgressIndicating
        {
            get { return ProgressIndicatingVal; }
            set => SetProperty(ref ProgressIndicatingVal, value);
        }

        private int ProgressVal;
        public int Progress
        {
            get { return ProgressVal; }
            set => SetProperty(ref ProgressVal, value);
        }

        private string ProgressTextVal;
        public string ProgressText
        {
            get { return ProgressTextVal; }
            set => SetProperty(ref ProgressTextVal, value);
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
