using BackendApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BackendApi.Services
{
    public class WeatherMonitorService
    {
        private readonly IHubContext<WeatherHub> _hubContext;
        private readonly System.Timers.Timer _timer;

        private Dictionary<string, string> _lastWeatherSummary = new();

        public WeatherMonitorService(IHubContext<WeatherHub> hubContext)
        {
            _hubContext = hubContext;
            _timer = new System.Timers.Timer(60000); // Check every 60 seconds (you can change this)
            _timer.Elapsed += async (sender, e) => await CheckForUpdates();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async Task CheckForUpdates()
        {
            // Example cities. In real app, fetch from Supabase DB
            var cities = new List<string> { "Ooty", "London" };

            foreach (var city in cities)
            {
                string newForecast = await GetForecastSummary(city); // Simulated logic

                if (_lastWeatherSummary.ContainsKey(city) && _lastWeatherSummary[city] != newForecast)
                {
                    // Notify all users (or a specific user with .User(userId))
                    await _hubContext.Clients.All.SendAsync("ReceiveWeatherUpdate", $"Weather changed in {city}: {newForecast}");
                }

                _lastWeatherSummary[city] = newForecast;
            }
        }

        private async Task<string> GetForecastSummary(string city)
        {
            // TODO: Call your actual Weather API here
            await Task.Delay(100); // simulate delay
            return DateTime.Now.Second % 2 == 0 ? "Rain" : "Sunny"; // simulate weather changes
        }
    }
}
