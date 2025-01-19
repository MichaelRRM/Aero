using Aero.Application;
using Aero.Base;
using Aero.Worker.WebApi;
using Asp.Versioning;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var aspnetCoreEnvironment = builder.Environment.EnvironmentName; 
builder.Configuration
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/apis.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/databases.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true);
    //.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(path: @$"{Path.GetTempPath()}/AeroLogs/aero.worker.webapi-.txt");
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, WorkerApiUserService>();
builder.Services.AddScoped<IWorkerRunner, WorkerRunner>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new QueryStringApiVersionReader("api-version")
    );
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o =>
    o.WithTheme(ScalarTheme.Default)
        .WithEndpointPrefix("api/{documentName}")
);

app.UseHttpsRedirection();

var v1Routes = app.NewVersionedApi("MyApi")
    .MapGroup("/api/v{version:apiVersion}")
    .HasApiVersion(new ApiVersion(1, 0));

v1Routes.ConfigureRoutes();

app.Run();