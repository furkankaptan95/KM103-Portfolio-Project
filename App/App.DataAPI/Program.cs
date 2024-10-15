using Microsoft.EntityFrameworkCore;
using App.Data.DbContexts;
using App.Services.AdminServices.Abstract;
using App.DataAPI.Services;
using App.Core.Validators.AboutMeValidators;
using FluentValidation;
using App.DTOs.AboutMeDtos;
using App.DTOs.BlogPostDtos;
using App.Core.Validators.BlogPostValidators;
using App.DTOs.EducationDtos;
using App.Core.Validators.EducationValidators;
using App.DTOs.ExperienceDtos;
using App.Core.Validators.ExperienceValidators;
using App.DTOs.PersonalInfoDtos;
using App.Core.Validators.PersonalInfoValidators;
using App.Core.Validators.ProjectValidators;
using App.DTOs.ProjectDtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authApiUrl = builder.Configuration.GetValue<string>("AuthApiUrl");

if (string.IsNullOrWhiteSpace(authApiUrl))
{
    throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
}

builder.Services.AddHttpClient("authApi", c =>
{
    c.BaseAddress = new Uri(authApiUrl);
});

builder.Services.AddDbContext<DataApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataApiBaseDb"));
});

builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IPersonalInfoService, PersonalInfoService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IHomeService, HomeService>();


builder.Services.AddTransient<IValidator<AddAboutMeApiDto>, AddAboutMeApiDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateAboutMeApiDto>, UpdateAboutMeApiDtoValidator>();
builder.Services.AddTransient<IValidator<AddBlogPostDto>, AddBlogPostDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateBlogPostDto>, UpdateBlogPostDtoValidator>();
builder.Services.AddTransient<IValidator<AddEducationDto>, AddEducationDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateEducationDto>, UpdateEducationDtoValidator>();
builder.Services.AddTransient<IValidator<AddExperienceDto>, AddExperienceDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateExperienceDto>, UpdateExperienceDtoValidator>();
builder.Services.AddTransient<IValidator<AddPersonalInfoDto>, AddPersonalInfoDtoValidator>();
builder.Services.AddTransient<IValidator<UpdatePersonalInfoDto>, UpdatePersonalInfoDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateProjectApiDto>, UpdateProjectApiDtoValidator>();
builder.Services.AddTransient<IValidator<AddProjectApiDto>, AddProjectApiDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<DataApiDbContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

app.Run();
