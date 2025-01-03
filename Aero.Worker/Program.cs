using Aero.Application;
using Aero.Base;
using Aero.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices();
builder.Services.AddScoped<IUserService, WorkerUserService>();
builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
host.Run();
