using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public interface ICompanyIdSaver: IIdMasterSaver<CompanyBusinessEntity>
{
}