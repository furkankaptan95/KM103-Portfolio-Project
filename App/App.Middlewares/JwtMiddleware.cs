using App.Services.AuthService.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace App.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            // IAuthService'i çözümle
            var authService = serviceProvider.GetService<IAuthService>();

            // Token işlemleri
            var jwtToken = context.Request.Cookies["JwtToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(jwtToken) && string.IsNullOrEmpty(refreshToken))
            {
                if (!context.Request.Path.StartsWithSegments("/Auth/Login"))
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            // JWT yoksa ve refresh token varsa, auth API'ye gidip yeni bir JWT ve refresh token al
            if (string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(refreshToken))
            {
                await RenewTokens(context, authService, refreshToken);

            }

            // Eğer JWT varsa, süresini kontrol et
            if (!string.IsNullOrEmpty(jwtToken))
            {
                // Token süresi dolmuş mu kontrol et
                if (TokenExpired(jwtToken))
                {
                    // JWT süresi dolmuş ve refresh token varsa, auth API'ye gidip yeni bir JWT ve refresh token al
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
                    // Eğer JWT geçerli ve süresi dolmamışsa, doğrulama yap
                    var isValidToken = await authService.ValidateTokenAsync(jwtToken);

                    if (!isValidToken.IsSuccess)
                    {
                        // JWT geçersiz, refresh token ile yeni token al
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

            // Eğer geçerli bir JWT varsa, devam et
            await _next(context);
        }

        private async Task RenewTokens(HttpContext context, IAuthService authService, string refreshToken)
        {
            var tokensResponse = await authService.RefreshTokenAsync(refreshToken);
            var tokens = tokensResponse.Value;

            if (tokens != null && !string.IsNullOrEmpty(tokens.JwtToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
            {
                // Yeni JWT ve refresh token'ı cookie'ye ekle
                context.Response.Cookies.Append("JwtToken", tokens.JwtToken);
                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken);
            }
            else
            {
                // Refresh token geçersizse, kullanıcıyı çıkışa yönlendir
                context.Response.Redirect("/Auth/Login");
                return;
            }
        }

        private bool TokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Token'ın geçerlilik süresi kontrol ediliyor
            var expirationDate = jwtToken.ValidTo;
            return expirationDate < DateTime.UtcNow;
        }
    }
}
