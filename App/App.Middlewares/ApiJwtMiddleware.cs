﻿using App.Core.Authorization;
using App.Services.AuthService.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.Middlewares;
public class ApiJwtMiddleware
{
    private readonly RequestDelegate _next;

    public ApiJwtMiddleware(RequestDelegate next)
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
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
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
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            else
            {
                var isValidToken = await authService.ValidateTokenAsync(jwtToken);

                if (isValidToken.IsSuccess)
                {

                    // JWT'den ClaimsPrincipal oluştur
                    var handler = new JwtSecurityTokenHandler();
                    var jwtTokenObject = handler.ReadToken(jwtToken) as JwtSecurityToken;

                    if (jwtTokenObject != null)
                    {
                        var identity = new ClaimsIdentity(jwtTokenObject.Claims, "jwt");
                        context.User = new ClaimsPrincipal(identity); // Kullanıcı bilgilerini ayarla
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
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
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

            if (!tokensResponse.IsSuccess)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var tokens = tokensResponse.Value;

            if (tokens != null && !string.IsNullOrEmpty(tokens.JwtToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
            {

                CookieOptions jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(10) // JWT ile aynı süre
                };

                // Refresh token için de süre ayarlanabilir
                CookieOptions refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7) // Refresh token süresi
                };


                context.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

                // JWT'den ClaimsPrincipal oluştur
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(tokens.JwtToken) as JwtSecurityToken;
                var identity = new ClaimsIdentity(jwtToken?.Claims, "jwt");
                context.User = new ClaimsPrincipal(identity); // Kullanıcı bilgilerini ayarla
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
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
                return true; // Geçersiz token formatı, bu durumda expired olarak işaretlenir.
            }

            var expirationDate = jwtToken.ValidTo;
            return expirationDate < DateTime.UtcNow;
        }
        catch (Exception)
        {
            return true; // Token geçersiz formatta olduğunda, expired olarak değerlendir.
        }
    }
}