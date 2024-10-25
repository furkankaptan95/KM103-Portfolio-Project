using Microsoft.AspNetCore.Http;

namespace App.Core;
public class AuthCookiesHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthCookiesHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Cookie'leri al
        var jwtToken = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

        // Cookie'leri ekle
        if (!string.IsNullOrEmpty(jwtToken))
        {
            request.Headers.Add("Cookie", $"JwtToken={jwtToken}");
        }

        if (!string.IsNullOrEmpty(refreshToken))
        {
            request.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
        }

        // İsteği gönder
        return await base.SendAsync(request, cancellationToken);
    }
}
