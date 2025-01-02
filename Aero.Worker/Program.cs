using Aero.Worker;
using Datahub.Application;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddServices();

var host = builder.Build();
host.Run();
