using Aero.Application.ApiServices;
using Aero.Application.ApiServices.Models;
using Aero.Base;

namespace Aero.WebApi;

public static class Routes
{
    private const string DealsTag = "Deals";
    private const string DataPointTag = "DataPoints";
    private const string UserTag = "User";
    private const string AdminTag = "Admin";

    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        webApplication.MapGet("/status", () => "api is working!")
            .WithTags(AdminTag)
            .WithOpenApi();
        
        webApplication.MapGet("/user", (IUserService userService) => userService.GetUserName())
            .WithTags(UserTag)
            .WithOpenApi();
        
        webApplication.MapGroup("/deals")
            .MapDealsApi()
            .WithTags(DealsTag)
            .WithOpenApi();
        
        webApplication.MapGroup("/data-points")
            .MapDataPointApi()
            .WithTags(DataPointTag)
            .WithOpenApi();
    }
    
    private static RouteGroupBuilder MapDealsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", (IDealsApiService dealsApiService, int id) => dealsApiService.GetDeal(id)).WithOpenApi();
        return group;
    }
    
    private static RouteGroupBuilder MapDataPointApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", (IDataPointApiService dataPointApiService) => dataPointApiService.GetDataPoints()).WithOpenApi();
        return group;
    }
}