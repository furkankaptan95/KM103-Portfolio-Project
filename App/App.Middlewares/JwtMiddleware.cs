using App.Core;
using App.Services.AuthService.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace App.Middlewares;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {

        var authService = serviceProvider.GetService<IAuthService>();
        
        var jwtToken = context.Request.Cookies["JwtToken"];
        var refreshToken = context.Request.Cookies["RefreshToken"];

        var endpoint = context.GetEndpoint();

        var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousManuelAttribute>() != null;

        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        if (string.IsNullOrEmpty(jwtToken) && string.IsNullOrEmpty(refreshToken))
        {
             context.Response.Redirect("/Auth/Login");
             return;
        }

        if (string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(refreshToken))
        {
            await RenewTokens(context, authService, refreshToken);

        }

        if (!string.IsNullOrEmpty(jwtToken))
        {
            if (TokenExpired(jwtToken))
            {

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await RenewTokens(context, authService, refreshToken);
                }
                else
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }
            else
            {               
                var isValidToken = await authService.ValidateTokenAsync(jwtToken);

                if (!isValidToken.IsSuccess)
                {
                    
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        await RenewTokens(context, authService, refreshToken);
                    }
                    else
                    {
                        context.Response.Redirect("/Auth/Login");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }

    private async Task RenewTokens(HttpContext context, IAuthService authService, string refreshToken)
    {
        try
        {
            var tokensResponse = await authService.RefreshTokenAsync(refreshToken);

            if(!tokensResponse.IsSuccess)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            var tokens = tokensResponse.Value;

            if (tokens != null && !string.IsNullOrEmpty(tokens.JwtToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
            {
                context.Response.Cookies.Append("JwtToken", tokens.JwtToken);
                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken);
            }
            else
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }
        }
       catch (Exception)
        {
            context.Response.Redirect("/Auth/Login");
            return;
        }
    }

    private bool TokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var expirationDate = jwtToken.ValidTo;
        return expirationDate < DateTime.UtcNow;
    }
}
