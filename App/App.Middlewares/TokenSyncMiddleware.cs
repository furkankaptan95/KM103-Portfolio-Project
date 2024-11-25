using Microsoft.AspNetCore.Http;

namespace App.Middlewares;
// MVC'de tokenları almak için middleware kullanabilirsiniz
public class TokenSyncMiddleware
{
    private readonly RequestDelegate _next;

    public TokenSyncMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // DataAPI'den gelen tokenları al
        var accessToken = context.Request.Cookies["access_token"];
        var refreshToken = context.Request.Cookies["refresh_token"];

        if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
        {
            // Token'ları MVC'ye kaydet
            context.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions 
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(10)

            });
            context.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            { 
                HttpOnly = true ,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)

            });
        }

        await _next(context);
    }
}

