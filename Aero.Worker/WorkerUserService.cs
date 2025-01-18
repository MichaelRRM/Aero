using Aero.Base;
using Aero.Base.Constants;

namespace Aero.Worker;

public class WorkerUserService : IUserService
{
    private readonly IConfiguration _configuration;

    public WorkerUserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetUserName()
    {
        return _configuration[$"{WorkerArgumentNames.UserName}"] ?? Environment.UserName;
    }
}