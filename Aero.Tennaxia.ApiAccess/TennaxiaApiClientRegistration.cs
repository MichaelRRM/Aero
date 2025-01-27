using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Tennaxia.ApiAccess;

public static class TennaxiaApiClientRegistration
{
    public static IServiceCollection AddTennaxiaApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<TennaxiaApiClient>(httpClient =>
        {
            var apiEnvironment = configuration[$"Environment:Apis:Tennaxia"] ?? throw new Exception($"Couldn't get api environment for Tennaxia");
            var url = configuration[$"Apis:Tennaxia:{apiEnvironment}:URL"] ?? throw new Exception($"Couldn't get api url for Tennaxia in environment {apiEnvironment}");
            var token = configuration[$"Apis:Tennaxia:{apiEnvironment}:Token"] ?? throw new Exception($"Couldn't get api token for Tennaxia in environment {apiEnvironment}");

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("private-token", token);
        });
        return services;
    }
}