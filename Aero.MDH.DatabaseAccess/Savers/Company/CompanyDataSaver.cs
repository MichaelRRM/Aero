using Aero.MDH.DatabaseAccess.BusinessEntities;
using Aero.MDH.DatabaseAccess.BusinessEntities.Base;
using Aero.MDH.DatabaseAccess.DataServices.Base;
using Aero.MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.Savers.Company;

public class CompanyDataSaver : DataModelSaver<CompanyBusinessEntity, CompanyDatum>, ICompanyDataSaver
{
    public CompanyDataSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<CompanyDatum> GetDbSet()
    {
        return DbContext.CompanyData;
    }
}