using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Xabe.FFmpeg.Downloader;
using YoutubeDLSharp.Options;
using YoutubeDLSharp;
using System.Diagnostics;
using Downloader.ViewModels.Pages;
using Downloader.Models;
using Downloader.Enums;
using Downloader.Helpers;
using Downloader.ViewModels.Controls.Observers;

namespace Downloader.ViewModels.Controls
{
    public class DownloaderItemControlVM : DownloaderItemControlOM
    {
        private bool _cancel = false;
        private CancellationTokenSource? _cts;

        public DownloaderItemControlVM(YoutubeVideoModel video, bool startDownloading, bool includeAudio)
        {
            Video = video;
            AvailebleFormats = FileFormatHelper.GetYtDlpFormats();
            SelectedFormat = AvailebleFormats.Any(x => x == FileFormatsEnum.Webm) ? FileFormatsEnum.Webm : AvailebleFormats.FirstOrDefault();
            IncludeAudio = includeAudio;

            #region COMMANDS
            StartDownloadCommand = new RelayCommand(StartDownload);
            RemoveDownloadCommand = new RelayCommand(RemoveDownload);
            RevealFileCommand = new RelayCommand(RevealFile);
            StopDownloadingCommand = new RelayCommand(StopDownloading);
            #endregion

            ProgressText = "Download";
            if (startDownloading) StartDownload();
        }

        #region COMMANDS
        public ICommand StartDownloadCommand { get; }
        public ICommand RemoveDownloadCommand { get; }
        public ICommand RevealFileCommand { get; }
        public ICommand StopDownloadingCommand { get; }
        #endregion

        #region Command Actions 
        public async void StartDownload()
        {
            var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads");
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            try
            {
                if (Downloading)
                {
                    StopDownloading();
                    return;
                }

                Completed = false;
                Downloading = true;
                ProgressText = "Loading ...";

                await Task.Delay(1000);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg")) || !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg", "ffmpeg.exe")))
                {
                    MessageBox.Show("Downloading ffmpeg, this may take some time.", "Info");
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg"))) Directory.CreateDirectory("ffmpeg");
                    await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, "ffmpeg");
                }

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "dlp")) || !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "dlp", "yt-dlp.exe")))
                {
                    MessageBox.Show("Downloading yt-dlp, this may take some time.", "Info");
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "dlp"))) Directory.CreateDirectory("dlp");
                    await YoutubeDLSharp.Utils.DownloadYtDlp(Path.Combine(Directory.GetCurrentDirectory(), "dlp"));
                }

                var ytdl = new YoutubeDL
                {
                    YoutubeDLPath = Path.Combine(Directory.GetCurrentDirectory(), "dlp", "yt-dlp.exe"),
                    FFmpegPath = Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg", "ffmpeg.exe"),
                    OutputFolder = outputPath,
                    OutputFileTemplate = "%(title)s.%(ext)s"
                };

                var options = new OptionSet
                {
                    Format = SelectedFormat.ToYtDlpFormat(IncludeAudio),
                    NoPlaylist = true,
                    Quiet = true,
                    NoWarnings = true,
                    AudioQuality = 0,

                    ExtractAudio = false,
                    MergeOutputFormat = SelectedFormat.ToYtDlpOutputMergeFormat(),
                    RecodeVideo = SelectedFormat.ToYtDlpRecodeVideoFormat()
                };

                if (SelectedFormat == FileFormatsEnum.Mp3)
                {
                    options.ExtractAudio = true;
                    options.AudioFormat = AudioConversionFormat.Mp3;
                }

                if (_cts != null) _cts?.Cancel();
                _cancel = false;
                _cts = new CancellationTokenSource();

                var progress = new Progress<DownloadProgress>(prog =>
                {
                    ProgressText = "Downloading ...";
                });

                var res = SelectedFormat == FileFormatsEnum.Mp3 ? await ytdl.RunAudioDownload(Video.Url, overrideOptions: options, progress: progress, ct: _cts.Token) : await ytdl.RunVideoDownload(Video.Url, overrideOptions: options, progress: progress, ct: _cts.Token);

                if (res.Success)
                {
                    DownloadedPath = res.Data;
                }
                else
                {
                    string errors = string.Join(Environment.NewLine, res.ErrorOutput);
                    MessageBox.Show(errors, "Error while downloading");
                }
            }
            catch (OperationCanceledException)
            {
                _cancel = true;
            }
            catch (Exception ex)
            {
                ProgressText = "Download";
                Completed = false;
                Downloading = false;
                MessageBox.Show(ex.Message, "Error while downloading");
            }
            finally
            {
                if (!_cancel)
                {
                    ProgressText = "Completed";
                    Completed = true;
                    Downloading = false;
                    _cts?.Cancel();
                }
            }
        }

        public void RemoveDownload()
        {
            try
            {
                _cts?.Cancel();
                var file = DownloaderPageVM.DownloadItems.FirstOrDefault(x => x.Video.Url == Video.Url);
                if (file != null) DownloaderPageVM.DownloadItems.Remove(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while removing");
            }
        }

        public void StopDownloading()
        {
            _cts?.Cancel();
            ProgressText = "Download";
            Completed = false;
            Downloading = false;
        }

        public void RevealFile()
        {
            if (!string.IsNullOrEmpty(DownloadedPath))
            {
                Process.Start("explorer.exe", $"/select,\"{DownloadedPath}\"");
            }
        }
        #endregion

    }
}
