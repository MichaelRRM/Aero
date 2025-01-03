using System.Security.Claims;
using Aero.Base;

namespace Aero.WebApi;

public class ApiUserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? Environment.UserName;
    }
}