using App.Core.Config;
using App.Core.Validators.ViewModelValidators.AboutMeValidators;
using App.DTOs;
using App.Services.AdminServices.Abstract;
using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace App.AdminMVC.Services;
public static class AdminMvcServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddJwtAuth(services, configuration);

        services.Configure<FileApiSettings>(configuration.GetSection("FileApiSettings"));
        services.AddControllersWithViews();
        services.AddValidatorsFromAssembly(typeof(AddAboutMeViewModelValidator).Assembly);
        services.AddHttpContextAccessor();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN"; // İsteğe bağlı olarak header adı
        });

        ConfigureHttpClients(services, configuration);

        RegisterScopedServices(services);

        return services;
    }

    private static void AddJwtAuth(IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions = configuration.GetSection("Jwt").Get<JwtTokenOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenOptions.Issuer,
                ValidateIssuer = true,
                ValidAudience = tokenOptions.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key)),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            options.MapInboundClaims = true;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies["JwtToken"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },


                OnChallenge = async context =>
                {
                    var refreshToken = context.Request.Cookies["RefreshToken"];

                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                        var result = await authService.RefreshTokenAsync(refreshToken);

                        if (result.IsSuccess)
                        {
                            context.Response.Cookies.Append("JwtToken", result.Value.JwtToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                Expires = DateTime.UtcNow.AddMinutes(10)
                            });

                            context.Response.Cookies.Append("RefreshToken", result.Value.RefreshToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                Expires = DateTime.UtcNow.AddDays(7)
                            });

                            context.HandleResponse();
                            return;
                        }
                    }

                    context.Response.Redirect("/Auth/Login"); // MVC'de Unauthorized sayfasına yönlendir
                    context.HandleResponse();
                },

                OnForbidden = context =>
                {
                    // Yetki sorunu varsa, MVC'ye yönlendirme yap
                    context.Response.Redirect("/Auth/AccessDenied"); // MVC'de Forbidden sayfasına yönlendir
                    return Task.CompletedTask; // Yönlendirme sonrası işlemi sonlandır
                }
            };
        });
    }

    private static void ConfigureHttpClients(IServiceCollection services, IConfiguration configuration)
    {
        var dataApiUrl = configuration.GetValue<string>("DataApiUrl");
        if (string.IsNullOrWhiteSpace(dataApiUrl))
        {
            throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
        }
        services.AddHttpClient("dataApi", c =>
        {
            c.BaseAddress = new Uri(dataApiUrl);
        })
         .ConfigurePrimaryHttpMessageHandler(() =>
         {
             var handler = new HttpClientHandler();

             // CookieContainer oluştur
             var cookieContainer = new CookieContainer();

             // HttpContext'ten cookie'leri al
             var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
             var jwtToken = httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
             var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

             // Cookie'leri ekle
             if (!string.IsNullOrEmpty(jwtToken))
             {
                 cookieContainer.Add(new Uri(dataApiUrl), new Cookie("JwtToken", jwtToken));
             }

             if (!string.IsNullOrEmpty(refreshToken))
             {
                 cookieContainer.Add(new Uri(dataApiUrl), new Cookie("RefreshToken", refreshToken));
             }

             handler.CookieContainer = cookieContainer; // CookieContainer'ı handler'a ekle
             return handler;
         });

        
        var fileApiUrl = configuration.GetValue<string>("FileApiUrl");
        if (string.IsNullOrWhiteSpace(fileApiUrl))
        {
            throw new InvalidOperationException("FileApiUrl is required in appsettings.json");
        }
        services.AddHttpClient("fileApi", c =>
        {
            c.BaseAddress = new Uri(fileApiUrl);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();

            // CookieContainer oluştur
            var cookieContainer = new CookieContainer();

            // HttpContext'ten cookie'leri al
            var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
            var jwtToken = httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
            var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

            // Cookie'leri ekle
            if (!string.IsNullOrEmpty(jwtToken))
            {
                cookieContainer.Add(new Uri(fileApiUrl), new Cookie("JwtToken", jwtToken));
            }

            if (!string.IsNullOrEmpty(refreshToken))
            {
                cookieContainer.Add(new Uri(fileApiUrl), new Cookie("RefreshToken", refreshToken));
            }

            handler.CookieContainer = cookieContainer; // CookieContainer'ı handler'a ekle
            return handler;
        });

        var authApiUrl = configuration.GetValue<string>("AuthApiUrl");
        if (string.IsNullOrWhiteSpace(authApiUrl))
        {
            throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
        }
        services.AddHttpClient("authApi", c =>
        {
            c.BaseAddress = new Uri(authApiUrl);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();

            // CookieContainer oluştur
            var cookieContainer = new CookieContainer();

            // HttpContext'ten cookie'leri al
            var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
            var jwtToken = httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
            var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

            // Cookie'leri ekle
            if (!string.IsNullOrEmpty(jwtToken))
            {
                cookieContainer.Add(new Uri(authApiUrl), new Cookie("JwtToken", jwtToken));
            }

            if (!string.IsNullOrEmpty(refreshToken))
            {
                cookieContainer.Add(new Uri(authApiUrl), new Cookie("RefreshToken", refreshToken));
            }

            handler.CookieContainer = cookieContainer; // CookieContainer'ı handler'a ekle
            return handler;
        });
    }

    private static void RegisterScopedServices(IServiceCollection services)
    {
        services.AddScoped<IAboutMeAdminService, AboutMeService>();
        services.AddScoped<IBlogPostAdminService, BlogPostService>();
        services.AddScoped<IEducationAdminService, EducationService>();
        services.AddScoped<IExperienceAdminService, ExperienceService>();
        services.AddScoped<IPersonalInfoAdminService, PersonalInfoService>();
        services.AddScoped<IProjectAdminService, ProjectService>();
        services.AddScoped<IHomeAdminService, HomeService>();
        services.AddScoped<ICommentAdminService, CommentService>();
        services.AddScoped<IUserAdminService, UserService>();
        services.AddScoped<IContactMessageAdminService, ContactMessageService>();
        services.AddScoped<IAuthService, AuthService>();
    }

}
