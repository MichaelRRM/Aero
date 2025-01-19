using System.Security.Claims;
using Aero.Base;

namespace Aero.Worker.WebApi.Services;

public class WorkerApiUserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkerApiUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? Environment.UserName;
    }
}