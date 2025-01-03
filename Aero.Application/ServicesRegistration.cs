using Aero.Application.ApiServices;
using Aero.Application.ApiServices.Models;
using Aero.Application.Workers;
using Aero.Application.Workers.TennaxiaDataCollection;
using Aero.Application.Workers.TennaxiaDataCollection.Modules;
using MDH.DatabaseAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMdhDbContext(configuration);
        
        //workers
        services.AddScoped<WorkerFactory>();
        services.AddScoped<TennaxiaDataCollection>();
        
        //modules
        services.AddScoped<DealDataFeederModule>();
        
        //api services 
        services.AddScoped<IDealsApiService, DealsApiService>();
        services.AddScoped<IDataPointApiService, DataPointApiService>();
        
        return services;
    } 
}