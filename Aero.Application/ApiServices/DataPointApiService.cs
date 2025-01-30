using Aero.Application.ApiServices.Models;
using Aero.Base.Attributes;

namespace Aero.Application.ApiServices;

[Injectable]
public class DataPointApiService
{
    public IEnumerable<DataPointDto> GetDataPoints()
    {
        return [];
    }
}