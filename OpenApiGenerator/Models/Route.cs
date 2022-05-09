namespace OpenApiGenerator.Models
{
    public class Route
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public IEnumerable<string> Metods { get; set; }
        public string UpStreamPath { get; set; }
        public string DownStreamPath { get; set; }
    }
}
