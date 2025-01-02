using Aero.Application.Workers;
using Aero.Application.Workers.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //workers
        services.AddScoped<TennaxiaDataCollection>();
        
        //modules
        services.AddScoped<DealDataFeederModule>();
        
        return services;
    } 
}