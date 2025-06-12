using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This makes the route: /api/my
    public class MyController : ControllerBase
    {
        private readonly SupabaseServices _supabaseServices;

        public MyController(SupabaseServices supabaseServices)
        {
            _supabaseServices = supabaseServices;
        }

        [HttpGet("test-supabase")]
        public async Task<IActionResult> TestSupabase()
        {
            await _supabaseServices.InitializeAsync();
            var client = _supabaseServices.Client;
            return Ok("Supabase Initialized");
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromForm]string email,[FromForm] string password)
        {
            await _supabaseServices.InitializeAsync();

            var result = await _supabaseServices.Client.Auth.SignUp(email, password);

            if(result?.User != null)
            {
                return Ok("user registered Successfully");
            }
            else
            {
                return BadRequest("Registration Failed");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            await _supabaseServices.InitializeAsync();

            var session = await _supabaseServices.Client.Auth.SignIn(email, password);

            if (session != null && session.User != null)
                return Ok(new { Token = session.AccessToken });
            else
                return Unauthorized("Login failed.");
        }


    }
}
