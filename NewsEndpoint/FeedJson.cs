using System.Text.Json;

namespace NewsEndpoint
{
    public class FeedJson
    {
        public string title { get; set; }
        public string desc { get; set; }
        public IEnumerable<Entry> entries { get; set; }

        public string ToJson() => JsonSerializer.Serialize(this, options: new JsonSerializerOptions() { WriteIndented = true });
    }

    public class Entry
    {
        public string title { get; set; }
        public string link { get; set; }
        public string pubdate { get; set; }
        public IEnumerable<string> authors { get; set; }
    }
}
