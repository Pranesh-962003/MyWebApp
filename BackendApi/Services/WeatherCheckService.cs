using BackendApi.Models;
using Supabase;
using System.Net.Http.Json;
using System.Text.Json;

namespace BackendApi.Services
{
    public class WeatherCheckService
    {
        private readonly SupabaseServices _supabaseService;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherCheckService(SupabaseServices supabaseService, IHttpClientFactory httpClientFactory)
        {
            _supabaseService = supabaseService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CheckTrackedForecasts()
        {
            var client = _supabaseService.Client;
            var trackedCities = await client
                .From<TrackedForecast>()
                .Get();

            foreach (var item in trackedCities.Models)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.City))
                        continue;

                    var httpClient = _httpClientFactory.CreateClient();
                    var apiKey = "14780708de535792e94ae35c2f22e2be"; // 🔥 Replace with real API key
                    var url = $"https://api.openweathermap.org/data/2.5/forecast?q={item.City}&appid={apiKey}&units=metric";

                    var response = await httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        continue;

                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherForeCastData>(content);

                    var latestWeather = weatherData?.List?.FirstOrDefault();

                    if (latestWeather != null && latestWeather.Weather.Any())
                    {
                        var newWeatherMain = latestWeather.Weather.First().Main;

                        if (newWeatherMain != item.WeatherMain)
                        {
                            item.WeatherMain = newWeatherMain;
                            item.LastUpdated = DateTime.UtcNow;

                            // Update the tracked forecast in the Supabase table
                            await client.From<TrackedForecast>().Update(item);

                            // TODO: trigger email notification if weather has changed (later)
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking weather: {ex.Message}");
                }
            }
        }
    }
}
