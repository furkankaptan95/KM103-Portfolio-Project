using App.Core.Authorization;
using App.Services.AdminServices.Abstract;
using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using App.ViewModels.AdminMvc.AboutMeViewModels;
using FluentValidation;
using System.Net;

namespace App.AdminMVC.Services;
public static class AdminMvcServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddValidatorsFromAssemblyContaining<AddAboutMeViewModel>();
        services.AddHttpContextAccessor();

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
        });

        
        var authApiUrl = configuration.GetValue<string>("AuthApiUrl");
        if (string.IsNullOrWhiteSpace(authApiUrl))
        {
            throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
        }
        services.AddHttpClient("authApi", c =>
        {
            c.BaseAddress = new Uri(authApiUrl);
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
