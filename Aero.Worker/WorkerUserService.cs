using Aero.Base;

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
        return _configuration["userName"] ?? System.Environment.UserName;
    }
}