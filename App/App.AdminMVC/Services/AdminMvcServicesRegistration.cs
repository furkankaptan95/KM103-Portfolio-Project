using App.Services.AdminServices.Abstract;

namespace App.AdminMVC.Services;
public static class AdminMvcServicesRegistration
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


        return services;

    }
}
