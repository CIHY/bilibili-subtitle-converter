using System.Text.Json.Serialization;

namespace BilibiliSubtitleConverter
{
    public class SourceFile
    {
        [JsonPropertyName("font_size")]
        public float FontSize { get; set; }

        [JsonPropertyName("font_color")]
        public string FontColor { get; set; }

        [JsonPropertyName("background_alpha")]
        public float BackgroundAlpha { get; set; }

        [JsonPropertyName("background_color")]
        public string BackgroundColor { get; set; }

        [JsonPropertyName("Stroke")]
        public string Stroke { get; set; }

        [JsonPropertyName("body")]
        public SourceFileBodyItem[] Body { get; set; }
    }

    public class SourceFileBodyItem
    {
        [JsonPropertyName("from")]
        public float From { get; set; }

        [JsonPropertyName("to")]
        public float To { get; set; }

        [JsonPropertyName("location")]
        public int Location { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
