using System.Text.Json.Serialization;

namespace Sauna.Core
{
    public class Temperature
    {
        [JsonPropertyName("k")]
        public double K { get; set; }

        [JsonPropertyName("f")]
        public double F { get; set; }

        [JsonPropertyName("c")]
        public double C { get; set; }
    }
}