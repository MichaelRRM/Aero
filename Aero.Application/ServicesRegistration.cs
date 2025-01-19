using Aero.Application.ApiServices;
using Aero.Application.ApiServices.Models;
using Aero.Application.Workers;
using Aero.Application.Workers.TennaxiaDataCollection;
using Aero.Application.Workers.TennaxiaDataCollection.Modules;
using MDH.DatabaseAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tennaxia.ApiAccess;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //api 
        services.AddTennaxiaApiClient(configuration);
        
        //databases
        services.AddMdhDbContext(configuration);
        
        //workers
        services.AddScoped<WorkerFactory>();
        services.AddScoped<TennaxiaDataCollectionWorker>();
        
        //modules
        services.AddScoped<DealDataFeederModule>();
        
        //api services 
        services.AddScoped<IDealsApiService, DealsApiService>();
        services.AddScoped<IDataPointApiService, DataPointApiService>();
        
        return services;
    } 
}