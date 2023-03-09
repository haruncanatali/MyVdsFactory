using System.Security.Claims;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.API.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        string UserIdStr = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = Convert.ToInt64(UserIdStr);
        IsAuthenticated = UserId != null;
    }

    public long UserId { get; }
    public bool IsAuthenticated { get; }
}