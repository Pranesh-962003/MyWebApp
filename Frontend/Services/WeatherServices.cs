using Frontend.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class WeatherServices
    {
        private readonly HttpClient _httpClient;

        private const string ApiKey = "14780708de535792e94ae35c2f22e2be";

        public WeatherServices(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<WeatherData?> GetWeatherData(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";
             return await _httpClient.GetFromJsonAsync<WeatherData>(url);
        }
    }
}
