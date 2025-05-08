using CommunityToolkit.Mvvm.Input;
using Downloader.Pages;
using Downloader.ViewModels.Controls;
using Downloader.ViewModels.Pages.Observers;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Downloader.ViewModels.Pages
{
    public class ConvertorPageVM : ConvertorPageOM
    {
        private ConvertorPage _view = null;
        public static ObservableCollection<ConvertorFileControlVM> ConvertItems { get; set; } = new();

        public ConvertorPageVM(ConvertorPage view)
        {
            _view = view;

            #region COMMANDS
            SelectFilesCommand = new RelayCommand(SelectFiles);
            #endregion
        }

        #region COMMANDS
        public ICommand SelectFilesCommand { get; }
        #endregion

        #region Command Actions 
        public void SelectFiles()
        {
            string[] files = new string[0];
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Title = "Select file(s) to convert",
                    Filter = "Convertible Files|*.mp4;*.mov;*.avi;*.mkv;*.flv;*.jpg;*.jpeg;*.png;*.bmp;*.tiff;*.gif;*.webp|All Files|*.*",
                    Multiselect = true
                };
                if (dlg.ShowDialog() == true)
                {
                    if (dlg.FileNames == null || !dlg.FileNames.Any())
                    {
                        MessageBox.Show("No files found.", "Error");
                        return;
                    }

                    files = dlg.FileNames;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while getting file");
            }
            LoadFiles(files);
        }
        #endregion

        #region Helpers
        private void LoadFiles(IEnumerable<string> filePaths)
        {
            //FileItems.Clear();
            foreach (var path in filePaths)
            {
                ConvertItems.Add(new ConvertorFileControlVM(path, ConvertOnSelect, IncludeAudio));
            }
        }
        #endregion

    }
}
