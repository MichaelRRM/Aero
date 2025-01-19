namespace Aero.Worker.WebApi;

public static class Routes
{
    private const string AdminTag = "Admin";
    public static void ConfigureRoutes(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapGet("/status", () => "worker api is working!")
            .WithTags(AdminTag)
            .WithOpenApi();
    }
}