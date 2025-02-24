using Aero.Application.ApiServices.Models;
using Aero.Base.Attributes;
using Aero.MDH.DatabaseAccess;

namespace Aero.Application.ApiServices;

[Injectable]
public class DealsApiService
{
    private readonly MdhDbContext _mdhDbContext;

    public DealsApiService(MdhDbContext mdhDbContext)
    {
        _mdhDbContext = mdhDbContext;
    }

    public DealDto GetDeal(int id)
    {
        return new DealDto(id, "test");
    }
}