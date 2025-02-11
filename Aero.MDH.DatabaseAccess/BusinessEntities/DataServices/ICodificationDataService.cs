using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;

public interface ICodificationDataService
{
    Dictionary<string, int> GetCodeToGlobalId(IEnumerable<string> codes, CodificationField codificationField);
}