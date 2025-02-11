using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;

public class CodificationDataService : ICodificationDataService
{
    private readonly MdhDbContext _dbContext;

    public CodificationDataService(MdhDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Dictionary<string, int> GetCodeToGlobalId(IEnumerable<string> codes, CodificationField codificationField)
    {
        var codificationCode = codificationField.Code;
        var codesHashSet = new HashSet<string?>(codes);
        
        // 2 calls are necessary, one to get all global ids potentially concerned, a second one to make sure we select only the last version of the code for each global id  
        var globalIdsToConsider = _dbContext.Codifications.AsNoTracking().Where(c => c.CodificationCode == codificationCode && codesHashSet.Contains(c.CodeTxt)).Select(c => c.GlobalId).ToHashSet();
        return _dbContext.Codifications.AsNoTracking().Where(c => c.CodificationCode == codificationCode && globalIdsToConsider.Contains(c.GlobalId))
            .GroupBy(c => c.GlobalId)
            .Select(g => g.OrderByDescending(c => c.VersionId).First())
            .ToDictionary(c => c.CodeTxt ?? string.Empty, c => c.GlobalId);
    } 
}