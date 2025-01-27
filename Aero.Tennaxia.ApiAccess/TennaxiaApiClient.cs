using System.Net.Http.Json;
using Aero.Tennaxia.ApiAccess.Model;

namespace Aero.Tennaxia.ApiAccess;

public class TennaxiaApiClient
{
    private readonly HttpClient _httpClient;

    public TennaxiaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TennaxiaApiResponse>?> GetAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TennaxiaApiResponse>>("repos/dotnet/AspNetCore.Docs/branches");
    }
}