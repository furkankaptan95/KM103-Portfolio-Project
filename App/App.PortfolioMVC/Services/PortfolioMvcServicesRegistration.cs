using App.Core.Authorization;
using App.Core.Config;
using App.Core.Validators.ViewModelValidators.UserValidators;
using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using App.Services.PortfolioServices.Abstract;
using FluentValidation;
using System.Net;

namespace App.PortfolioMVC.Services;

public static class PortfolioMvcServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileApiSettings>(configuration.GetSection("FileApiSettings"));
        services.AddControllersWithViews();
        services.AddValidatorsFromAssembly(typeof(EditUserImageViewModelValidator).Assembly);
        services.AddHttpContextAccessor();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN"; // İsteğe bağlı olarak header adı
        });

        services.AddScoped<AuthorizationService>();

        ConfigureHttpClients(services, configuration);
        RegisterScopedServices(services);

        return services;
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
        services.AddScoped<IAboutMePortfolioService, AboutMePortfolioService>();
        services.AddScoped<IBlogPostPortfolioService, BlogPostPortfolioService>();
        services.AddScoped<ICommentPortfolioService, CommentPortfolioService>();
        services.AddScoped<IEducationPortfolioService, EducationPortfolioService>();
        services.AddScoped<IExperiencePortfolioService, ExperiencePortfolioService>();
        services.AddScoped<IPersonalInfoPortfolioService, PersonalInfoPortfolioService>();
        services.AddScoped<IProjectPortfolioService, ProjectPortfolioService>();
        services.AddScoped<IHomePortfolioService, HomePortfolioService>();
        services.AddScoped<IContactMessagePortfolioService, ContactMessagePortfolioService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserPortfolioService, UserPortfolioService>();
    }
}
