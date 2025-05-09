using System.IO;
using System.Windows;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Downloader.Helpers
{
    public class DownloadHelpers
    {
        private static readonly SemaphoreSlim ytdlpSemaphore = new(1, 1);
        public static async Task<DownloadStatus> DownloadYtdlp(bool forceDownload = false)
        {
            string ytdlpPath = Path.Combine(Directory.GetCurrentDirectory(), "dlp");
            await ytdlpSemaphore.WaitAsync(); //w8 op een release
            try
            {
                bool alreadyInstalled = !forceDownload
                    && Directory.Exists(ytdlpPath)
                    && File.Exists(Path.Combine(ytdlpPath, "yt-dlp.exe"));

                if (alreadyInstalled) return DownloadStatus.Ready;
                if (forceDownload && Directory.Exists(ytdlpPath)) Directory.Delete(ytdlpPath, true);

                MessageBox.Show("Downloading yt-dlp, this may take some time.", "Info");
                if (!Directory.Exists(ytdlpPath)) Directory.CreateDirectory(ytdlpPath);
                await YoutubeDLSharp.Utils.DownloadYtDlp(ytdlpPath);
                await Task.Delay(3000);

                return DownloadStatus.Ready;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error downloading ytdlp");
                return DownloadStatus.Failed;
            }
            finally
            {
                ytdlpSemaphore.Release();//release
            }
        }

        private static readonly SemaphoreSlim ffmpegSemaphore = new(1, 1);
        public static async Task<DownloadStatus> DownloadFfmpeg(bool forceDownload = false)
        {
            string ffmpegDownloadPath = Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg");
            string ffmPegPath = "ffmpeg";

            await ffmpegSemaphore.WaitAsync(); //w8 op een release
            try
            {
                bool alreadyInstalled = !forceDownload &&
                    Directory.Exists(ffmpegDownloadPath) &&
                    File.Exists(Path.Combine(ffmpegDownloadPath, "ffmpeg.exe")) &&
                    File.Exists(Path.Combine(ffmpegDownloadPath, "ffprobe.exe"));

                if (alreadyInstalled)
                {
                    FFmpeg.SetExecutablesPath(ffmPegPath);
                    return DownloadStatus.Ready;
                }

                if (forceDownload && Directory.Exists(ffmpegDownloadPath))Directory.Delete(ffmpegDownloadPath, true);

                MessageBox.Show("Downloading ffmpeg, this may take some time.", "Info");
                if (!Directory.Exists(ffmpegDownloadPath))Directory.CreateDirectory(ffmpegDownloadPath);
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmPegPath);
                await Task.Delay(3000);

                FFmpeg.SetExecutablesPath(ffmPegPath);
                return DownloadStatus.Ready;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error downloading ffmpeg");
                return DownloadStatus.Failed;
            }
            finally
            {
                ffmpegSemaphore.Release();//release
            }
        }
    }
    public enum DownloadStatus
    {
        Ready,
        Failed,
    }

}
