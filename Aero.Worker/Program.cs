using Aero.Application;
using Aero.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices();
builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
host.Run();
