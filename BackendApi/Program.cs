using BackendApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.Text;
using BackendApi.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add controllers for API endpoints
builder.Services.AddControllers(); // Only one AddControllers needed

builder.Services.AddHttpClient();
// Register your services for Dependency Injection (DI)
// Use AddSingleton since the service doesn't need to be scoped per request
builder.Services.AddSingleton<SupabaseServices>(); // Singleton to initialize once
builder.Services.AddScoped<FavoriteCityService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<WeatherCheckService>(); // just singleton
builder.Services.AddHostedService<WeatherCheckBackgroundService>(); // hosted is the BackgroundService
builder.Services.AddSingleton<EmailSenderService>(); // for emails

builder.Services.AddHostedService<WeatherCheckBackgroundService>();
builder.Services.AddSingleton<EmailSenderService>();
builder.Services.AddSingleton<MdbSettingsService>();
builder.Services.AddSingleton<WeatherForecastService>();

// Enable logging (optional)
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Adding console logging
});

// CORS setup to allow specific origins for security
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:3000"); // Adjust for security, e.g., React app's URL
    });
});

// JWT Authentication setup for Supabase
var supabaseJwtSecret = "GKQ7EuoBylI2MDpwFS3zmROZeWjwLZ0f+GSXu/ZO6NyisdakFDAWF94NruvLmVpcqUye+t83QcYVnpdZDaJ4DA==";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://hnyufgmenktqtmvdwlco.supabase.co/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://hnyufgmenktqtmvdwlco.supabase.co/auth/v1",
            ValidateAudience = true,
            ValidAudience = "authenticated",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseJwtSecret)),
            ClockSkew = TimeSpan.Zero // Optional: adjust if your token expiration time is strict
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log any authentication failures
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Log successful token validation
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token successfully validated.");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddSingleton<WeatherMonitorService>();

builder.Services.AddAuthorization();

var app = builder.Build();
var weatherMonitorService = app.Services.GetRequiredService<WeatherMonitorService>();

// Initialize Supabase services after the app starts
var supabaseService = app.Services.GetRequiredService<SupabaseServices>();
await supabaseService.InitializeAsync(); // Initialize Supabase asynchronously

// Middleware pipeline setup
app.UseHttpsRedirection();
app.UseCors(); // Enable CORS
app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization


// Map controllers (API endpoints)
app.MapControllers();
app.MapHub<WeatherHub>("/weatherHub");
// Run the application
app.Run();
