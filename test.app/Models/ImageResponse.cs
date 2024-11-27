using System.Text.Json.Serialization;

namespace test.app.Models
{
    public class ImageResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }
}
