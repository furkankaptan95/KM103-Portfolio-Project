using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Core;
public class AuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizationService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public int GetAuthorizationStatus(IEnumerable<string> roles)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("Unauthorized access attempt.");
            return StatusCodes.Status401Unauthorized;
        }

        var userRoles = user.Claims
            .Where(c => c.Type == "role")
            .Select(c => c.Value)
            .ToList();

        if (!roles.Intersect(userRoles).Any())
        {
            _logger.LogWarning("Access denied. Required roles: {Roles}, User roles: {UserRoles}", roles, userRoles);
            return StatusCodes.Status403Forbidden;
        }

        return StatusCodes.Status200OK;
    }
}

