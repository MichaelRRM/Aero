using Aero.Worker.WebApi.Models;
using Aero.Worker.WebApi.Services;

namespace Aero.Worker.WebApi;

public static class Routes
{
    private const string AdminTag = "Admin";
    private const string WorkersTag = "Workers";
    
    public static void ConfigureRoutes(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapGet("/status", () => "worker api is working!")
            .WithTags(AdminTag)
            .WithOpenApi();
        
        routeGroup.MapGroup("/workers")
            .MapWorkersApi()
            .WithTags(WorkersTag)
            .WithOpenApi();
    }
    
    private static RouteGroupBuilder MapWorkersApi(this RouteGroupBuilder group)
    {
        group.MapGet("", (Aero.Worker.WebApi.Services.WorkerService workersService) => workersService.GetWorkers()).WithOpenApi();
        group.MapPost("", (WorkerRunner workerRunner, WorkerLaunchRequest workerRequest) => workerRunner.RunWorkerOutOfProcess(workerRequest)).WithOpenApi();
        return group;
    }
}