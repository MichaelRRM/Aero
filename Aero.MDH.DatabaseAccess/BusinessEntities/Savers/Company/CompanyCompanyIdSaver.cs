using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;
using Aero.MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyCompanyIdSaver : IdMasterSaver<CompanyBusinessEntity, CompanyIdMaster>, ICompanyIdSaver
{
    public CompanyCompanyIdSaver(MdhDbContext mdhDbContext) : base(mdhDbContext)
    {
    }

    protected override DbSet<CompanyIdMaster> GetDbSet(MdhDbContext mdhDbContext)
    {
        return mdhDbContext.CompanyIdMasters;
    }

    protected override CompanyIdMaster ConvertToDatabaseModel(CompanyBusinessEntity abstractBusinessEntity)
    {
        return new CompanyIdMaster
        {
            CompanyId = abstractBusinessEntity.Id,
            CompanyType = abstractBusinessEntity.CompanyType ?? throw new ArgumentNullException(nameof(abstractBusinessEntity.CompanyType)),
        };
    }
}