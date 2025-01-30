using Aero.Application;
using Aero.Base;
using Aero.Worker;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

var aspnetCoreEnvironment = builder.Environment.EnvironmentName; 
builder.Configuration
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/apis.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/databases.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/Configuration/environment.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true);
    //.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddScoped<IUserService, WorkerUserService>();
builder.Services.AddHostedService<WorkerService>();
builder.Services.AddLogging(logBuilder => logBuilder.AddSerilog());

var host = builder.Build();
host.Run();