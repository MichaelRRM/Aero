using Aero.Application;
using Aero.Base;
using Aero.Worker.WebApi;

var builder = WebApplication.CreateBuilder(args);

var aspnetCoreEnvironment = builder.Environment.EnvironmentName; 
builder.Configuration
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/apis.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/databases.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true);
    //.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, WorkerApiUserService>();
builder.Services.AddScoped<IWorkerRunner, WorkerRunner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}