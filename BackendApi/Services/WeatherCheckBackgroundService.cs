using Microsoft.Extensions.Hosting;

namespace BackendApi.Services
{
    public class WeatherCheckBackgroundService : BackgroundService
    {
        private readonly WeatherCheckService _weatherCheckService;
        private readonly ILogger<WeatherCheckBackgroundService> _logger;

        public WeatherCheckBackgroundService(WeatherCheckService weatherCheckService, ILogger<WeatherCheckBackgroundService> logger)
        {
            _weatherCheckService = weatherCheckService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather Check Background Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _weatherCheckService.CheckTrackedForecasts();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking tracked forecasts");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // 🔥 every 5 min
            }
        }
    }
}
