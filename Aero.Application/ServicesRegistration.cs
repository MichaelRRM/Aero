using Aero.Application.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //modules
        services.AddScoped<DealFeederModule>();
        
        return services;
    } 
}