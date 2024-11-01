using App.Core.Validators.DtoValidators.AuthValidators;
using App.Core.Validators.DtoValidators.UserValidators;
using App.Data.DbContexts;
using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using App.Services;
using App.Services.AdminServices.Abstract;
using App.Services.AuthService.Abstract;
using App.Services.PortfolioServices.Abstract;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.AuthAPI.Services;
public static class AuthApiServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // CORS yapılandırması
        ConfigureCors(services);

        services.AddHttpContextAccessor();
        // Temel yapılandırmalar
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // HttpClient yapılandırması
        ConfigureHttpClient(services, configuration);

        // DbContext yapılandırması
        ConfigureDbContext(services, configuration);

        // Hizmetleri ekleme
        ConfigureServices(services);

        // Validator'ları ekleme
        ConfigureValidators(services);

        return services;
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:7071", "https://localhost:7241", "https://localhost:7167", "https://localhost:7085")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        var dataApiUrl = configuration.GetValue<string>("DataApiUrl");

        if (string.IsNullOrWhiteSpace(dataApiUrl))
        {
            throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
        }

        services.AddHttpClient("dataApi", c =>
        {
            c.BaseAddress = new Uri(dataApiUrl);
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
                cookieContainer.Add(new Uri(dataApiUrl), new Cookie("JwtToken", jwtToken));
            }

            if (!string.IsNullOrEmpty(refreshToken))
            {
                cookieContainer.Add(new Uri(dataApiUrl), new Cookie("RefreshToken", refreshToken));
            }

            handler.CookieContainer = cookieContainer; // CookieContainer'ı handler'a ekle
            return handler;
        });
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthApiDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AuthApiBaseDb"));
        });
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IEmailService, SmtpEmailService>();
        services.AddScoped<IUserAdminService, AdminUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserPortfolioService, PorfolioUserService>();
    }

    private static void ConfigureValidators(IServiceCollection services)
    {
        services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
        services.AddTransient<IValidator<ForgotPasswordDto>, ForgotPasswordDtoValidator>();
        services.AddTransient<IValidator<RenewPasswordDto>, RenewPasswordDtoValidator>();
        services.AddTransient<IValidator<NewPasswordDto>, NewPasswordDtoValidator>();
        services.AddTransient<IValidator<RegisterDto>, RegisterDtoValidator>();
        services.AddTransient<IValidator<VerifyEmailDto>, VerifyEmailDtoValidator>();
        services.AddTransient<IValidator<EditUserImageApiDto>, EditUserImageApiDtoValidator>();
        services.AddTransient<IValidator<EditUsernameDto>, EditUsernameDtoValidator>();
        services.AddTransient<IValidator<NewVerificationMailDto>, NewVerificationMailDtoValidator>();
        
    }
}
