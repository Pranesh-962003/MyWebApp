using System.Text.Json.Serialization;

namespace Frontend.Models
{

    public class WeatherData
    {
        public string Name { get; set; } = string.Empty;

        public MainData Main { get; set; } = new();

        public List<WeatherInfo> Weather { get; set; } = new();
    }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public float Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
