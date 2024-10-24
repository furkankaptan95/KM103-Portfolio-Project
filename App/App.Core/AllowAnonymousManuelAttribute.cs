using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Core;
public class AllowAnonymousManuelAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
    }
}
