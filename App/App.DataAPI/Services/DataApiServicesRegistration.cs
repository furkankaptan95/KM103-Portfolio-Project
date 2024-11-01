using App.Core.Validators.DtoValidators.AboutMeValidators;
using App.Core.Validators.DtoValidators.BlogPostValidators;
using App.Core.Validators.DtoValidators.CommentValidators;
using App.Core.Validators.DtoValidators.ContactMessageValidators;
using App.Core.Validators.DtoValidators.EducationValidators;
using App.Core.Validators.DtoValidators.ExperienceValidators;
using App.Core.Validators.DtoValidators.PersonalInfoValidators;
using App.Core.Validators.DtoValidators.ProjectValidators;
using App.Data.DbContexts;
using App.DataAPI.Services.AdminServices;
using App.DTOs.AboutMeDtos.Admin;
using App.DTOs.AboutMeDtos;
using App.DTOs.BlogPostDtos.Admin;
using App.DTOs.CommentDtos.Portfolio;
using App.DTOs.ContactMessageDtos.Admin;
using App.DTOs.ContactMessageDtos.Portfolio;
using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos.Admin;
using App.DTOs.ExperienceDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.ProjectDtos.Admin;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using App.Services.AuthService.Abstract;
using App.Services.PortfolioServices.Abstract;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using App.Services.AuthService.Concrete;
using App.DataAPI.Services.PortfolioServices;
using App.Core.Authorization;
using System.Net;
using App.Services;
using App.DataAPI.Services;

namespace App.DataApi.Services;
public static class DataApiServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin() // Tüm origin'lere izin verir.
                       .AllowAnyMethod() // Tüm HTTP yöntemlerine izin verir.
                       .AllowAnyHeader(); // Tüm başlıklara izin verir.
            });
        });

        // Temel yapılandırmalar
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // DbContext yapılandırması
        ConfigureDbContext(services, configuration);

        services.AddScoped<AuthorizationService>();

        ConfigureHttpClients(services, configuration);

        services.AddSingleton<IEmailService, SmtpEmailService>();
        // Scoped hizmetler
        RegisterScopedServices(services);

        // Validator'ları ekleme
        RegisterValidators(services);

        return services;
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataApiDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DataApiBaseDb"));
        });
    }

    private static void ConfigureHttpClients(IServiceCollection services, IConfiguration configuration)
    {
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
        // Admin hizmetleri
        services.AddScoped<IAboutMeAdminService, AboutMeAdminService>();
        services.AddScoped<IBlogPostAdminService, BlogPostAdminService>();
        services.AddScoped<ICommentAdminService, CommentAdminService>();
        services.AddScoped<IEducationAdminService, EducationAdminService>();
        services.AddScoped<IExperienceAdminService, ExperienceAdminService>();
        services.AddScoped<IPersonalInfoAdminService, PersonalInfoAdminService>();
        services.AddScoped<IProjectAdminService, ProjectAdminService>();
        services.AddScoped<IHomeAdminService, HomeAdminService>();
        services.AddScoped<IContactMessageAdminService, ContactMessageAdminService>();
        services.AddScoped<IAuthService, AuthService>();

        // Portfolio hizmetleri
        services.AddScoped<IAboutMePortfolioService, AboutMePortfolioService>();
        services.AddScoped<IBlogPostPortfolioService, BlogPostPortfolioService>();
        services.AddScoped<ICommentPortfolioService, CommentPortfolioService>();
        services.AddScoped<IEducationPortfolioService, EducationPortfolioService>();
        services.AddScoped<IExperiencePortfolioService, ExperiencePortfolioService>();
        services.AddScoped<IPersonalInfoPortfolioService, PersonalInfoPortfolioService>();
        services.AddScoped<IProjectPortfolioService, ProjectPortfolioService>();
        services.AddScoped<IContactMessagePortfolioService, ContactMessagePortfolioService>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddTransient<IValidator<AddAboutMeApiDto>, AddAboutMeApiDtoValidator>();
        services.AddTransient<IValidator<UpdateAboutMeApiDto>, UpdateAboutMeApiDtoValidator>();
        services.AddTransient<IValidator<AddBlogPostDto>, AddBlogPostDtoValidator>();
        services.AddTransient<IValidator<UpdateBlogPostDto>, UpdateBlogPostDtoValidator>();
        services.AddTransient<IValidator<AddEducationDto>, AddEducationDtoValidator>();
        services.AddTransient<IValidator<UpdateEducationDto>, UpdateEducationDtoValidator>();
        services.AddTransient<IValidator<AddExperienceDto>, AddExperienceDtoValidator>();
        services.AddTransient<IValidator<UpdateExperienceDto>, UpdateExperienceDtoValidator>();
        services.AddTransient<IValidator<AddPersonalInfoDto>, AddPersonalInfoDtoValidator>();
        services.AddTransient<IValidator<UpdatePersonalInfoDto>, UpdatePersonalInfoDtoValidator>();
        services.AddTransient<IValidator<UpdateProjectApiDto>, UpdateProjectApiDtoValidator>();
        services.AddTransient<IValidator<AddProjectApiDto>, AddProjectApiDtoValidator>();
        services.AddTransient<IValidator<AddCommentSignedDto>, AddCommentSignedDtoValidator>();
        services.AddTransient<IValidator<AddCommentUnsignedDto>, AddCommentUnsignedDtoValidator>();
        services.AddTransient<IValidator<AddContactMessageDto>, AddContactMessageDtoValidator>();
        services.AddTransient<IValidator<ReplyContactMessageDto>, ReplyContactMessageDtoValidator>();
    }
}
