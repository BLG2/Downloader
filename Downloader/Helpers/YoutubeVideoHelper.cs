using Downloader.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace Downloader.Helpers
{
    public class YoutubeVideoHelper
    {
        public static async Task<YoutubeVideoModel?> GetYoutubeVidInfo(string videoUrl)
        {
            try
            {
                string oEmbedUrl = $"https://www.youtube.com/oembed?url={videoUrl}&format=json";
                using var client = new HttpClient();
                string? jsonRes = await client.GetStringAsync(oEmbedUrl);
                YoutubeVideoModel? deserialized = JsonConvert.DeserializeObject<YoutubeVideoModel>(jsonRes);
                if (deserialized != null) deserialized.Url = videoUrl;
                return deserialized;
            }
            catch { }
            return null;

        }
    }
}
