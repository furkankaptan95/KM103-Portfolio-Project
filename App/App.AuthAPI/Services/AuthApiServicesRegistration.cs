using App.Data.DbContexts;
using App.Services.AdminServices.Abstract;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Services;
public static class AuthApiServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:7071") // İzin verilen köken
                       .AllowAnyMethod() // İzin verilen HTTP yöntemleri
                       .AllowAnyHeader(); // İzin verilen başlıklar
            });
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var dataApiUrl = configuration.GetValue<string>("DataApiUrl");

        if (string.IsNullOrWhiteSpace(dataApiUrl))
        {
            throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
        }

        services.AddHttpClient("dataApi", c =>
        {
            c.BaseAddress = new Uri(dataApiUrl);
        });


        services.AddDbContext<AuthApiDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AuthApiBaseDb"));
        });

        services.AddScoped<IUserAdminService, AdminUserService>();

        return services;
    }
}
