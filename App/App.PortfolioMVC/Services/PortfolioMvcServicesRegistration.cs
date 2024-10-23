using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using App.Services.PortfolioServices.Abstract;

namespace App.PortfolioMVC.Services;
public static class PortfolioMvcServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        var dataApiUrl = configuration.GetValue<string>("DataApiUrl");

        if (string.IsNullOrWhiteSpace(dataApiUrl))
        {
            throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
        }

        services.AddHttpClient("dataApi", c =>
        {
            c.BaseAddress = new Uri(dataApiUrl);
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

        return services;
    }
}
