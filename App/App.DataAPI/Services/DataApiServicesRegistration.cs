﻿using App.Core.Validators.AboutMeValidators;
using App.Core.Validators.BlogPostValidators;
using App.Core.Validators.EducationValidators;
using App.Core.Validators.ExperienceValidators;
using App.Core.Validators.PersonalInfoValidators;
using App.Core.Validators.ProjectValidators;
using App.Data.DbContexts;
using App.DataAPI.Services.AdminServices;
using App.DataAPI.Services.PortfolioServices;
using App.DTOs.AboutMeDtos;
using App.DTOs.AboutMeDtos.Admin;
using App.DTOs.BlogPostDtos.Admin;
using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos;
using App.DTOs.ExperienceDtos.Admin;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.DTOs.ProjectDtos;
using App.DTOs.ProjectDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
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

        services.AddScoped<IAboutMeAdminService, AboutMeAdminService>();
        services.AddScoped<IBlogPostAdminService, BlogPostAdminService>();
        services.AddScoped<ICommentAdminService, CommentAdminService>();
        services.AddScoped<IEducationAdminService, EducationAdminService>();
        services.AddScoped<IExperienceAdminService, ExperienceAdminService>();
        services.AddScoped<IPersonalInfoAdminService, PersonalInfoAdminService>();
        services.AddScoped<IProjectAdminService, ProjectAdminService>();
        services.AddScoped<IHomeAdminService, HomeAdminService>();
      

        services.AddScoped<IAboutMePortfolioService, AboutMePortfolioService>();
        services.AddScoped<IBlogPosPortfolioService, BlogPosPortfolioService>();
        services.AddScoped<ICommentPortfolioService, CommentPortfolioService>();
        services.AddScoped<IEducationPortfolioService, EducationPortfolioService>();
        services.AddScoped<IExperiencePortfolioService, ExperiencePortfolioService>();
        services.AddScoped<IPersonalInfoPortfolioService, PersonalInfoPortfolioService>();
        services.AddScoped<IProjectPortfolioService, ProjectPortfolioService>();


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