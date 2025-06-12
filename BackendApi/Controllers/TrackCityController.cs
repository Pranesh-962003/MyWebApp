// Path: Controllers/WeatherController.cs
using Microsoft.AspNetCore.Mvc;
using BackendApi.Models;
using BackendApi.Services;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/track-city")]
    public class WeatherController : ControllerBase
    {
        private readonly SupabaseServices _supabaseService;

        public WeatherController(SupabaseServices supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpPost]
        public async Task<IActionResult> TrackCity([FromBody] TrackCityRequest request)
        {
            // 1. Get the current user's ID from Supabase auth
            var user = _supabaseService.Client.Auth.CurrentUser;
            if (user == null)
                return Unauthorized("User must be logged in.");

            // 2. Create the TrackedForecast object
            var trackedForecast = new TrackedForecast
            {
                City = request.City,
                
                WeatherMain = "", // Leave empty for now, update later
                LastUpdated = DateTime.UtcNow
            };

            // 3. Insert into Supabase database
            var response = await _supabaseService.Client
                .From<TrackedForecast>()
                .Insert(trackedForecast);

            // 4. Return success message
            return Ok(new { message = "City is now being tracked." });
        }
    }

    public class TrackCityRequest
    {
        public string City { get; set; } = string.Empty;
    }
}
