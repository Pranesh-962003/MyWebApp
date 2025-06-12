using Microsoft.AspNetCore.SignalR;

namespace BackendApi.Hubs
{
    public class WeatherHub : Hub
    {
        public async Task SendWeatherAlert(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveWeatherUpdate", message);
        }
    }
}