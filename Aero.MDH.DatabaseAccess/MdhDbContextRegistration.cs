using Aero.Base;
using Aero.MDH.DatabaseAccess.DataServices;
using Aero.MDH.DatabaseAccess.Savers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.MDH.DatabaseAccess;

public static class MdhDbContextRegistration
{
    public static IServiceCollection AddMdhDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ConnectionHelper.GetConnectionString(configuration, "MDH");
        
        services.AddDbContext<MdhDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ICompanyDataService, CompanyDataService>();

        services.AddScoped<ICompanyGlobalIdSaver, CompanyGlobalIdSaver>();
        services.AddScoped<ICompanyIdSaver, CompanyIdSaver>();
        services.AddScoped<ICompanyCodificationSaver, CompanyCodificationSaver>();
        services.AddScoped<ICompanyDataSaver, CompanyDataSaver>();
        
        return services;
    }
}