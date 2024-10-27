using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Core.Authorization;
public class CommonAreaAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {

    }
}
