using System.Reflection;
using Aero.Application.ApiServices;
using Aero.Application.ApiServices.Models;
using Aero.Application.Workers;
using Aero.Application.Workers.TennaxiaDataCollection;
using Aero.Application.Workers.TennaxiaDataCollection.Modules;
using Aero.Base.Attributes;
using Aero.EFront.DataAccess;
using Aero.MDH.DatabaseAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aero.Tennaxia.ApiAccess;

namespace Aero.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //data access
        services.AddTennaxiaApiClient(configuration);
        services.AddMdhDbContext(configuration);
        services.AddScoped<IEFrontCompanyDataService, EFrontCompanyDataService>();
        
        services.AddInjectableServices();
        
        return services;
    }
    
    private static IServiceCollection AddInjectableServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.GetName().Name!.StartsWith("Aero."));

        foreach (var assembly in assemblies)
        {
            var injectableTypes = assembly.GetTypes()
                .Where(t => 
                    t is { IsClass: true, IsAbstract: false }
                    && t.GetCustomAttribute<InjectableAttribute>(inherit: true) != null
                );

            foreach (var type in injectableTypes)
            {
                services.AddScoped(type);
            }
        }

        return services;
    }
}