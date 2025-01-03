using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDH.DatabaseAccess;

public static class MdhDbContextRegistration
{
    public static IServiceCollection AddMdhDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MdhConnectionString");
        
        services.AddDbContext<MdhDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}