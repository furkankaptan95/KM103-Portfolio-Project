using App.Core.Validators.DtoValidators.AuthValidators;
using App.Data.DbContexts;
using App.DTOs.AuthDtos;
using App.Services;
using App.Services.AdminServices.Abstract;
using App.Services.AuthService.Abstract;
using App.Services.PortfolioServices.Abstract;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Services
{
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
                    builder.WithOrigins("https://localhost:7071")
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
        }
    }
}
