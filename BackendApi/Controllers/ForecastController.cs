using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using BackendApi.Models;
using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private WeatherForeCastData? newForecast;
        private readonly MdbSettingsService _mongo;
        private readonly WeatherForecastService _service;
        public ForecastController(MdbSettingsService mongo, WeatherForecastService service)
        {
            _mongo = mongo;
            _service = service;
        }
        [HttpPost("forecast")]
        public async Task<IActionResult> Get([FromBody] List<MCollection> forecastEntries) {
            var email = GetEmailFromToken();
            if (email == null) return Unauthorized();

            forecastEntries.ForEach(entry => entry.Email = email); // Ensure secure assignment


            await _mongo.SaveForecastAsync(forecastEntries);


            return Ok("Forecast saved to MongoDB.");
        }


        [HttpGet("getdata")]
        public async Task<IActionResult> getData()
        {
            var email = GetEmailFromToken();
            if (email == null) return Unauthorized();

            var forecasts = await _mongo.GetForecastByEmailAsync(email!);
            if (!forecasts.Any()) return NotFound("No forecast data found for user.");

            var city = forecasts.First().CityName;
            var lastStoredDate = forecasts.Max(f => f.Date.Date);
            var today = DateTime.UtcNow.Date;

            // Loop until the current date exceeds the last stored forecast date
            while (today <= lastStoredDate)
            {
                Console.WriteLine($"Checking weather for {today:yyyy-MM-dd}");

                // Fetch the latest forecast
                var newForecast = await _service.Get5DayForecastAsync(city);

                var daysToCompare = newForecast!.List
                    .Where(f => f.Dt.Date >= today)
                    .GroupBy(f => f.Dt.Date)
                    .Select(g => g.First())
                    .ToList();

                foreach (var forecast in daysToCompare)
                {
                    var storedEntry = forecasts.FirstOrDefault(f => f.Date.Date == forecast.Dt.Date);

                    if (storedEntry != null)
                    {
                        Console.WriteLine($"Comparing {storedEntry.Date:yyyy-MM-dd} (Stored: {storedEntry.Summary}) vs {forecast.Dt:yyyy-MM-dd} (Fetched: {forecast.Weather[0].Main})");

                        if (storedEntry.Summary != forecast.Weather[0].Main)
                        {

                            try
                            {
                                var smtpClient = new SmtpClient("smtp.gmail.com") // e.g., smtp.gmail.com
                                {
                                    Port = 587,
                                    Credentials = new NetworkCredential("pranesh962003.p@gmail.com", "kdmpmtkhjkdxlfna"),
                                    EnableSsl = true,
                                };

                                var mailMessage = new MailMessage
                                {
                                    From = new MailAddress("pranesh962003.p@gmail.com"),
                                    Subject = "the weather has been changed",
                                    Body = $"The weather for the city{city} has been changed {storedEntry.Date:yyyy-MM-dd} : {storedEntry.Summary} aand the current data is {forecast.Weather[0].Main}",
                                    IsBodyHtml = false,
                                };
                                mailMessage.To.Add(email);

                                smtpClient.Send(mailMessage);

                            }
                            catch (Exception ex)
                            {
                                return StatusCode(500, "Error sending email: " + ex.Message);
                            }// Trigger notification here
                        }
                        else
                        {
                            Console.WriteLine("no");
                        }
                    }
                    else
                    {
                        Console.WriteLine("no");
                    }
                }

                // Wait for 12 hours
                Console.WriteLine("Waiting 12 hours before next check...");
                await Task.Delay(TimeSpan.FromHours(12));

                today = DateTime.UtcNow.Date;
            }

            return Ok("Finished checking until the last stored forecast date.");
        }



        private string? GetEmailFromToken()
        {
            var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                // Safely retrieve the email from the token claims
                var email = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

                return email; // null will be returned if email is not found
            }
            catch
            {
                // In case of any exceptions, return null
                return null;
            }
        }

    }
}
