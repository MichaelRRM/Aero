using Aero.MDH.DatabaseAccess.BusinessEntities;

namespace Aero.MDH.DatabaseAccess.DataServices;

public interface ICompanyDataService
{
    CompanySaveRequest CreateOrUpdate(ICollection<CompanyBusinessEntity> companies);
}