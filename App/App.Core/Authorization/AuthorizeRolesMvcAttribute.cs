using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core.Authorization;

public class AuthorizeRolesMvcAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeRolesMvcAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var authorizationService = serviceProvider.GetService<AuthorizationService>();

        if (authorizationService != null)
        {
            var statusCode = authorizationService.GetAuthorizationStatus(_roles);

            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            else if (statusCode == StatusCodes.Status403Forbidden)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            }
        }
    }
}
