using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Core.Authorization;
public class PostCommentAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {

    }
}
