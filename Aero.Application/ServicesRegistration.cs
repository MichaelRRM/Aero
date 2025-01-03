using Aero.Application.Workers;
using Aero.Application.Workers.TennaxiaDataCollection;
using Aero.Application.Workers.TennaxiaDataCollection.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //workers
        services.AddScoped<WorkerFactory>();
        services.AddScoped<TennaxiaDataCollection>();
        
        //modules
        services.AddScoped<DealDataFeederModule>();
        
        return services;
    } 
}