using Aero.Application.ApiServices.Models;
using Aero.MDH.DatabaseAccess;

namespace Aero.Application.ApiServices;

public class DealsApiService : IDealsApiService
{
    private readonly MdhDbContext _mdhDbContext;

    public DealsApiService(MdhDbContext mdhDbContext)
    {
        _mdhDbContext = mdhDbContext;
    }

    public DealDto GetDeal(int id)
    {
        var dealDatas = _mdhDbContext.DealData.ToList();
        
        return new DealDto(id, "test");
    }
}