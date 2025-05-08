using Downloader.Enums;
using System.IO;
using YoutubeDLSharp.Options;

namespace Downloader.Helpers
{
    public static class FileFormatHelper
    {
        public static IEnumerable<FileFormatsEnum> GetAudioOnlyFormats()
        {
            return new List<FileFormatsEnum> {
                FileFormatsEnum.Mp3,
            };
        }

        public static IEnumerable<FileFormatsEnum> GetVideoAudioFormats()
        {
            return new List<FileFormatsEnum> {
                FileFormatsEnum.Mp4,
                FileFormatsEnum.Mkv,
                FileFormatsEnum.Ogg,
                FileFormatsEnum.Webm,
                FileFormatsEnum.Flv,
                FileFormatsEnum.Avi,
                FileFormatsEnum.Mp3,
            };
        }

        public static IEnumerable<FileFormatsEnum> GetYtDlpFormats()
        {
            return new List<FileFormatsEnum> {
                FileFormatsEnum.Webm,
                FileFormatsEnum.Mp4,
                FileFormatsEnum.Mp3,
            };
        }

        public static IEnumerable<FileFormatsEnum> GetImageFormats()
        {
            return new List<FileFormatsEnum> {
                FileFormatsEnum.Png,
                FileFormatsEnum.Jpg,
                FileFormatsEnum.Webp,
            };
        }

        public static IEnumerable<FileFormatsEnum> GetAvailebleFormats(this FileFormatsEnum? inputFormat)
        {
            if (inputFormat != null)
                switch (inputFormat)
                {
                    case FileFormatsEnum.Mp4:
                    case FileFormatsEnum.Mov:
                    case FileFormatsEnum.Avi:
                    case FileFormatsEnum.Mkv:
                    case FileFormatsEnum.Flv:
                    case FileFormatsEnum.Webm:
                        return GetVideoAudioFormats();

                    case FileFormatsEnum.Mp3:
                        return GetAudioOnlyFormats();

                    case FileFormatsEnum.Png:
                    case FileFormatsEnum.Jpg:
                    case FileFormatsEnum.Webp:
                        return GetImageFormats();

                }

            var defaultList = Enum.GetValues<FileFormatsEnum>().ToList();
            defaultList.Remove(FileFormatsEnum.Default);
            return defaultList;
        }

        public static FileFormatsEnum? StringExtentionToFormatEnum(this string inputPathOrExtention)
        {
            try
            {
                string extention = Path.GetExtension(inputPathOrExtention).Replace(".", "");
                Enum.TryParse(extention, true, out FileFormatsEnum parsedFormat);
                return parsedFormat;
            }
            catch { }
            return null;
        }

        public static DownloadMergeFormat ToYtDlpOutputMergeFormat(this FileFormatsEnum format)
        {
            switch (format)
            {
                case FileFormatsEnum.Mp4:
                    return DownloadMergeFormat.Mp4;
                case FileFormatsEnum.Webm:
                    return DownloadMergeFormat.Webm;
                default:
                    return DownloadMergeFormat.Unspecified;
            }
        }

        public static VideoRecodeFormat ToYtDlpRecodeVideoFormat(this FileFormatsEnum format)
        {
            switch (format)
            {
                case FileFormatsEnum.Mp4:
                    return VideoRecodeFormat.Mp4;
                case FileFormatsEnum.Webm:
                    return VideoRecodeFormat.Webm;
                default:
                    return VideoRecodeFormat.None;
            }
        }

        public static string ToYtDlpFormat(this FileFormatsEnum format, bool includeAudio)
        {
            switch (format)
            {
                case FileFormatsEnum.Mp4:
                    return includeAudio ? "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]" : "bestvideo[ext=mp4]/best[ext=mp4]";
                case FileFormatsEnum.Webm:
                    return includeAudio ? "bestvideo[ext=webm]+bestaudio[ext=webm]/best[ext=webm]" : "bestvideo[ext=webm]/best[ext=webm]";
                case FileFormatsEnum.Mp3:
                    return "bestaudio[ext=mp3]/bestaudio";
                default:
                    throw new NotSupportedException("Format not supported for yt-dlp.");
            }
        }

    }
}
