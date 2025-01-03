namespace Aero.Application.ApiServices.Models;

public interface IDataPointApiService
{
    IEnumerable<DataPointDto> GetDataPoints();
}