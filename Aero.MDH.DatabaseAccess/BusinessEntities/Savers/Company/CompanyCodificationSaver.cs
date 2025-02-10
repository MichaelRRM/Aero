﻿using Aero.MDH.DatabaseAccess.AutoGenerated.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Base;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyCodificationSaver : CodificationModelSaver<CompanyBusinessEntity>, ICompanyCodificationSaver
{
    public CompanyCodificationSaver(AutoGenerated.MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override IEnumerable<Codification> ConvertToDatabaseModels(CompanyBusinessEntity businessEntity)
    {
        aaa
    }
}