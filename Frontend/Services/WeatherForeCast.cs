using Frontend.Models;
namespace Frontend.Services
{
    public class WeatherForeCast
    {
        private readonly HttpClient _httpClient;

        private string ApiKey = "14780708de535792e94ae35c2f22e2be";
            public WeatherForeCast(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<WeatherForeCastData?> Get5DayForecastAsync(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={ApiKey}&units=metric";
            return await _httpClient.GetFromJsonAsync<WeatherForeCastData>(url);
        }

    }
}
