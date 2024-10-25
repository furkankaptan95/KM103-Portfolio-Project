using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace App.Core;
public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeRolesAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // JwtToken'ı Cookie'den alıyoruz
        var jwtToken = context.HttpContext.Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(jwtToken))
        {
            // Token yoksa 401 durumu ile yanıt ver
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        // Token'ı çözümle
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(jwtToken) as JwtSecurityToken;

        if (token == null || token.Claims == null)
        {
            // Token geçersizse 401 durumu ile yanıt ver
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        // Kullanıcının rollerini al
        var userRoles = token.Claims
         .Where(c => c.Type == "role") // Burada doğrudan "role" kullanmalısınız
         .Select(c => c.Value)
         .ToList();

        // Eğer kullanıcı gerekli rollerden birine sahip değilse
        if (!_roles.Any(role => userRoles.Contains(role)))
        {
            // Yeterli rol yoksa 403 durumu ile yanıt ver ve farklı bir sayfaya yönlendir
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            return;
        }
    }
}
