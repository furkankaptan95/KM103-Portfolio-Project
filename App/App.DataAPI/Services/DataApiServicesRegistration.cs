using App.Core.Validators.AboutMeValidators;
using App.Core.Validators.BlogPostValidators;
using App.Core.Validators.EducationValidators;
using App.Core.Validators.ExperienceValidators;
using App.Core.Validators.PersonalInfoValidators;
using App.Core.Validators.ProjectValidators;
using App.Data.DbContexts;
using App.DTOs.AboutMeDtos;
using App.DTOs.BlogPostDtos;
using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public static class DataApiServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:7163") // İzin verilen köken
                       .AllowAnyMethod() // İzin verilen HTTP yöntemleri
                       .AllowAnyHeader(); // İzin verilen başlıklar
            });
        });

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var authApiUrl = configuration.GetValue<string>("AuthApiUrl");

        if (string.IsNullOrWhiteSpace(authApiUrl))
        {
            throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
        }

        services.AddHttpClient("authApi", c =>
        {
            c.BaseAddress = new Uri(authApiUrl);
        });

        services.AddDbContext<DataApiDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DataApiBaseDb"));
        });

        services.AddScoped<IAboutMeService, AboutMeService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IPersonalInfoService, PersonalInfoService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<ICommentService, CommentService>();


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

        return services;
    }
}
