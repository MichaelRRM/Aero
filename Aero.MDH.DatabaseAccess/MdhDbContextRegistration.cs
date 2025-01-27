using Aero.Base;
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
        return services;
    }
}