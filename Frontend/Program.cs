using Frontend.Services;

using Frontend.Components;

using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UserSessionService>();

builder.Services.AddScoped<WeatherServices>();
builder.Services.AddScoped<FavouriteCityAddService>();
builder.Services.AddSingleton<SignalRService>();
builder.Services.AddScoped<WeatherForeCast>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://mywebapp-2.onrender.com/") });
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddMudServices();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
