using Aero.Application;
using Aero.Base;
using Aero.WebApi;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => {
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(path: @$"{Path.GetTempPath()}/AeroLogs/aero.webapi-.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, ApiUserService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o => 
    o.WithTheme(ScalarTheme.Default)
        .WithEndpointPrefix("api/{documentName}")
);

app.ConfigureRoutes();
app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.Run();