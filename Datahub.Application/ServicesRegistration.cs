using Datahub.Application.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Datahub.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //modules
        services.AddScoped<DealFeederModule>();
        
        return services;
    } 
}