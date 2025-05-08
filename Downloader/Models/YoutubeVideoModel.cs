using Newtonsoft.Json;

namespace Downloader.Models
{
    public class YoutubeVideoModel
    {
        public string Url { get; set; } = null!;
        public string Title { get; set; } = null!;
        [JsonProperty("author_name")]
        public string? AuthorName { get; set; }
        [JsonProperty("author_url")]
        public string? AuthorUrl { get; set; }
        [JsonProperty("thumbnail_url")]
        public string? Thumbnail { get; set; }
    }
}
