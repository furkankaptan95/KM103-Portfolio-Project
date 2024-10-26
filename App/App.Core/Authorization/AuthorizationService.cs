using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Core.Authorization;
public class AuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizationService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetAuthorizationStatus(IEnumerable<string> roles)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            return StatusCodes.Status401Unauthorized;
        }

        var userRoles = user.Claims
            .Where(c => c.Type == "role")
            .Select(c => c.Value)
            .ToList();

        if (!roles.Intersect(userRoles).Any())
        {
            return StatusCodes.Status403Forbidden;
        }

        return StatusCodes.Status200OK;
    }
}

