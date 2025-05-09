using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Xabe.FFmpeg;
using System.Diagnostics;
using Downloader.ViewModels.Pages;
using Downloader.Enums;
using Downloader.Helpers;
using Downloader.ViewModels.Controls.Observers;

namespace Downloader.ViewModels.Controls
{
    public class ConvertorFileControlVM : ConvertorFileControlOM
    {
        private bool _cancel = false;
        private CancellationTokenSource? _cts;
        private IConversion? _conversion;

        public ConvertorFileControlVM(string filePath, bool startConverting, bool audioOnly)
        {
            ProgressText = "Convert";

            FileFormatsEnum? fileFormat = filePath.StringExtentionToFormatEnum();
            AvailebleFormats = fileFormat.GetAvailebleFormats();
            SelectedFormat = AvailebleFormats.Any(x => x == FileFormatsEnum.Webm) ? FileFormatsEnum.Webm : AvailebleFormats.Any(x => x == FileFormatsEnum.Webp) ? FileFormatsEnum.Webp : AvailebleFormats.FirstOrDefault();

            #region COMMANDS
            ConvertFileCommand = new RelayCommand(ConvertFile);
            RemoveFileCommand = new RelayCommand(RemoveFile);
            RevealFileCommand = new RelayCommand(RevealFile);
            StopConvertingCommand = new RelayCommand(StopConverting);
            #endregion

            FileName = Path.GetFileName(filePath).ToUpper();
            FilePath = filePath;
            IncludeAudio = audioOnly;
            if (startConverting) ConvertFile();
        }

        #region COMMANDS
        public ICommand ConvertFileCommand { get; }
        public ICommand RemoveFileCommand { get; }
        public ICommand StopConvertingCommand { get; }
        public ICommand RevealFileCommand { get; }
        #endregion

        #region Command Actions 
        public async void ConvertFile()
        {
            var fileWithoutExtention = Path.GetFileNameWithoutExtension(FilePath);
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Conversions");
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            var outputPath = Path.Combine(outputDir, $"{fileWithoutExtention}.{SelectedFormat.ToString().ToLower()}");

            try
            {
                if (Converting)
                {
                    StopConverting();
                    return;
                }

                Completed = false;
                Converting = true;
                ProgressText = "Loading ...";

                await Task.Delay(1000);

                DownloadStatus ffmpegAvaileble = await DownloadHelpers.DownloadFfmpeg();
                if (ffmpegAvaileble != DownloadStatus.Ready)
                {
                    StopConverting();
                    return;
                }

                if (File.Exists(outputPath)) File.Delete(outputPath);

                ProgressText = $"0%";
                Progress = 0;

                if (_cts != null) _cts?.Cancel();
                _cancel = false;
                _cts = new CancellationTokenSource();
                _conversion = await FFmpeg.Conversions.FromSnippet.Convert(FilePath, outputPath);
                _conversion.OnProgress += (s, e) =>
                {
                    Progress = e.Percent;
                    ProgressText = $"{e.Percent}%";
                };
                await _conversion.Start(_cts.Token);
                ConvertedFilePath = outputPath;
            }
            catch (OperationCanceledException)
            {
                _cancel = true;
            }
            catch (Exception ex)
            {
                Progress = 0;
                ProgressText = "Convert";
                Completed = false;
                Converting = false;
                MessageBox.Show(ex.Message, "Error while converting");
            }
            finally
            {
                if (!_cancel)
                {
                    Progress = 100;
                    ProgressText = "Completed";
                    Completed = true;
                    Converting = false;
                    _cts?.Cancel();
                    _conversion = null;
                }
            }
        }

        public void RemoveFile()
        {
            try
            {
                _cts?.Cancel();
                _conversion = null;
                var file = ConvertorPageVM.ConvertItems.FirstOrDefault(x => x.FilePath == FilePath);
                if (file != null) ConvertorPageVM.ConvertItems.Remove(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while removing");
            }
        }

        public void StopConverting()
        {
            Progress = 0;
            ProgressText = "Convert";
            Completed = false;
            Converting = false;

            _cts?.Cancel();
            _conversion = null;
        }

        public void RevealFile()
        {
            var filePath = !string.IsNullOrEmpty(ConvertedFilePath) ? Path.GetFullPath(ConvertedFilePath) : Path.GetFullPath(FilePath);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
        #endregion

    }
}
