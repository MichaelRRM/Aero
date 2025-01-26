using Aero.Application;
using Aero.Base;
using Aero.WebApi;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
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
        .WriteTo.File(path: @$"{Path.GetTempPath()}/AeroLogs/aero.webapi-.txt");
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, ApiUserService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
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
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // the Angular dev server
            .AllowAnyMethod()
            .AllowAnyHeader()
            ;
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost4200");

app.UseSwagger();
app.UseSwaggerUI();

var v1Routes = app.NewVersionedApi()
    .MapGroup("/api/v{version:apiVersion}")
    .HasApiVersion(new ApiVersion(1, 0));

v1Routes.ConfigureRoutes();
app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.Run();