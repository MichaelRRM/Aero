using Aero.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;
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
        services.AddScoped<ICodificationDataService, CodificationDataService>();

        services.AddScoped<ICompanyGlobalIdSaver, CompanyGlobalIdSaver>();
        services.AddScoped<ICompanyIdSaver, CompanyCompanyIdSaver>();
        services.AddScoped<ICompanyCodificationSaver, CompanyCodificationSaver>();
        services.AddScoped<ICompanyDataSaver, CompanyDataSaver>();
        
        return services;
    }
}