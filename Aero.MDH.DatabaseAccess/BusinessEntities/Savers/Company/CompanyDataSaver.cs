using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

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