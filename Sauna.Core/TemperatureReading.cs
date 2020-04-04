using System;
using System.Text.Json.Serialization;

namespace Sauna.Core
{
    public class TemperatureReading
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("internal")]
        public Temperature Internal { get; set; }

        [JsonPropertyName("external")]
        public Temperature External { get; set; }
    }
}
