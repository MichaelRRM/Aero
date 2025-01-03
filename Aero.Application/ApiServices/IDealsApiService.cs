using Aero.Application.ApiServices.Models;

namespace Aero.Application.ApiServices;

public interface IDealsApiService
{
    DealDto GetDeal(int id);
}