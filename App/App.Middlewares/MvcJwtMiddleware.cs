using App.Core.Authorization;
using App.Services.AuthService.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.Middlewares;
public class MvcJwtMiddleware
{
    private readonly RequestDelegate _next;

    public MvcJwtMiddleware(RequestDelegate next)
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

        var postComment = endpoint?.Metadata.GetMetadata<CommonAreaAttribute>() != null;

        if (postComment)
        {
            if (string.IsNullOrEmpty(jwtToken) && string.IsNullOrEmpty(refreshToken))
            {
                await _next(context);
                return;
            }

            if (string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(refreshToken))
            {
                await RenewTokensAllow(context, authService, refreshToken);
            }

            if (!string.IsNullOrEmpty(jwtToken))
            {
                if (TokenExpired(jwtToken))
                {
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        await RenewTokensAllow(context, authService, refreshToken);
                    }
                    else
                    {
                        await _next(context);
                        return;
                    }
                }
                else
                {
                    var isValidToken = await authService.ValidateTokenAsync(jwtToken);

                    if (isValidToken.IsSuccess)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtTokenObject = handler.ReadToken(jwtToken) as JwtSecurityToken;

                        if (jwtTokenObject != null)
                        {
                            var identity = new ClaimsIdentity(jwtTokenObject.Claims, "jwt");
                            context.User = new ClaimsPrincipal(identity);
                        }
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            await RenewTokensAllow(context, authService, refreshToken);
                        }
                        else
                        {
                            await _next(context);
                            return;
                        }
                    }

                }
            }

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

                if (isValidToken.IsSuccess)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtTokenObject = handler.ReadToken(jwtToken) as JwtSecurityToken;

                    if (jwtTokenObject != null)
                    {
                        var identity = new ClaimsIdentity(jwtTokenObject.Claims, "jwt");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }

                else
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

                CookieOptions jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                };

                CookieOptions refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                context.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(tokens.JwtToken) as JwtSecurityToken;
                var identity = new ClaimsIdentity(jwtToken?.Claims, "jwt");
                context.User = new ClaimsPrincipal(identity);
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
    private async Task RenewTokensAllow(HttpContext context, IAuthService authService, string refreshToken)
    {
        try
        {
            var tokensResponse = await authService.RefreshTokenAsync(refreshToken);

            if (!tokensResponse.IsSuccess)
            {
                await _next(context);
                return;
            }

            var tokens = tokensResponse.Value;

            if (tokens != null && !string.IsNullOrEmpty(tokens.JwtToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
            {

                CookieOptions jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                };

                CookieOptions refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };


                context.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(tokens.JwtToken) as JwtSecurityToken;
                var identity = new ClaimsIdentity(jwtToken?.Claims, "jwt");
                context.User = new ClaimsPrincipal(identity);
            }
            else
            {
                await _next(context);
                return;
            }
        }
        catch (Exception)
        {
            await _next(context);
            return;
        }
    }
    private bool TokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return true;
            }

            var expirationDate = jwtToken.ValidTo;
            return expirationDate < DateTime.UtcNow;
        }
        catch (Exception)
        {
            return true;
        }
    }
}
