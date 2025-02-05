using Aero.EFront.DataAccess.Model;

namespace Aero.EFront.DataAccess;

public class EFrontCompanyDataService : IEFrontCompanyDataService
{
    public IEnumerable<EFrontCompany> GetEFrontCompanies()
    {
        return new List<EFrontCompany>(); 
    }
}