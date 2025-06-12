using System.Text.Json.Serialization;
using Org.BouncyCastle.Asn1.Pkcs;

namespace BackendApi.Models
{
    public class WeatherForeCastData
    {
        [JsonPropertyName("city")]
        public CityInfo City { get; set; } = new();

        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; } = new();
    }

    public class CityInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ForecastItem
    {
        [JsonPropertyName("main")]
        public MainData Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; } = new();

        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime Dt => DateTime.TryParse(DtTxt, out var date) ? date : DateTime.MinValue;
    }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
