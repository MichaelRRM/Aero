using Aero.EFront.DataAccess.Model;

namespace Aero.EFront.DataAccess;

public interface IEFrontCompanyDataService
{
    IEnumerable<EFrontCompany> GetEFrontCompanies();
}