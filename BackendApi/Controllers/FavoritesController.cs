using BackendApi.Models;
using BackendApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteCityController : ControllerBase
    {
        private readonly FavoriteCityService _favoriteCityService;

        public FavoriteCityController(FavoriteCityService favoriteCityService)
        {
            _favoriteCityService = favoriteCityService;
        }


        [HttpGet("check")]
        public IActionResult CheckApiStatus()
        {
            return Ok("API is working properly.");
        }

        // Add a favorite city for a user
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddFavoriteCity([FromBody] FavoriteCityRequest model)
        {
            var userId = GetUserIdFromToken();

            Console.WriteLine("UserID is" + userId);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token.");
            }

            // Check if the CityName is null or empty
            if (string.IsNullOrEmpty(model.CityName))
            {
                return BadRequest("City name is required.");
            }

            Console.WriteLine($"{model.CityName}");

            var result = await _favoriteCityService.AddFavoriteCityAsync(userId, model.CityName);
            if (result)
            {
                return Ok("City added successfully.");
            }

            return BadRequest("Failed to add city.");
        }


        // Get all favorite cities for a user
        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteCities()
        {
            var userId = GetUserIdFromToken();

            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token.");
            }

            var cities = await _favoriteCityService.GetFavoriteCitiesAsync(userId);
            if (cities == null || !cities.Any())
            {
                return NotFound("No favorite cities found.");
            }

            return Ok(cities);
        }

        // Helper method to extract user ID from the JWT token
        private string? GetUserIdFromToken()
        {
            var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);
          
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                // Safely retrieve the userId, return null if it's not available
                var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
               
                return userId; // null will be returned if userId is not found
            }
            catch
            {
                // In case of any exceptions, return null
                return null;
            }
        }


    }

    public class FavoriteCityRequest
    {
        public string? CityName { get; set; }
    }
}
