namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;

public interface ICompanyDataService
{
    CompanySaveRequest CreateOrUpdate(ICollection<CompanyBusinessEntity> companies);
}