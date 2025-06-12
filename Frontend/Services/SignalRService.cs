using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class SignalRService
    {
        private readonly HubConnection _hubConnection;

        // Constructor to initialize the HubConnection
        public SignalRService()
        {
            // Make sure you replace the URL with your actual backend URL
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7224/weatherHub")  // Replace with your backend URL
                .Build();
        }

        // Start the connection if it's not already in a connected or connecting state
        public async Task StartAsync()
        {
            // Check if the connection is in a disconnected state before starting
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                // Register for the "ReceiveWeatherUpdate" message from the server
                _hubConnection.On<string>("ReceiveWeatherUpdate", (message) =>
                {
                    Console.WriteLine($"Weather update: {message}");
                    // You can use this to show the update in your UI
                    // For example, you can invoke a method or display a notification to the user
                });

                try
                {
                    // Start the connection
                    await _hubConnection.StartAsync();
                    Console.WriteLine("SignalR connection started successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting connection: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Hub connection is already in a connected or connecting state.");
            }
        }

        // Stop the connection if it's currently connected
        public async Task StopAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
                Console.WriteLine("SignalR connection stopped.");
            }
            else
            {
                Console.WriteLine("Connection is already stopped or disconnected.");
            }
        }

        // Optionally, you can check the connection state
        public string GetConnectionState()
        {
            return _hubConnection.State.ToString();
        }
    }
}
