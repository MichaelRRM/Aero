using Microsoft.Extensions.Configuration;

namespace Aero.Base;

public static class ConnectionHelper
{
    public static string? GetConnectionString(IConfiguration configuration, string databaseName)
    {
        var databaseEnvironment = configuration[$"Environment:Databases:{databaseName}"];
        var database = configuration[$"ConnectionStrings:{databaseName}:{databaseEnvironment}"];
        return database;
    }
}