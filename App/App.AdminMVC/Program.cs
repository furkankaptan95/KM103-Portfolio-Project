using App.AdminMVC.Services;
using App.Core.Validators.AboutMeValidators;
using App.Core.Validators.BlogPostValidators;
using App.DTOs.AboutMeDtos;
using App.DTOs.BlogPostDtos;
using App.DTOs.FileApiDtos;
using App.Services;
using App.Services.AdminServices.Abstract;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);

var dataApiUrl = builder.Configuration.GetValue<string>("DataApiUrl");

if (string.IsNullOrWhiteSpace(dataApiUrl))
{
    throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
}

builder.Services.AddHttpClient("dataApi", c =>
{
    c.BaseAddress = new Uri(dataApiUrl);
});

var fileApiUrl = builder.Configuration.GetValue<string>("FileApiUrl");

if (string.IsNullOrWhiteSpace(fileApiUrl))
{
    throw new InvalidOperationException("FileApiUrl is required in appsettings.json");
}

builder.Services.AddHttpClient("fileApi", c =>
{
    c.BaseAddress = new Uri(fileApiUrl);
});

builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();



builder.Services.AddTransient<IValidator<AddAboutMeMVCDto>, AddAboutMeMVCDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateAboutMeMVCDto>, UpdateAboutMeMVCDtoValidator>();
builder.Services.AddTransient<IValidator<AddBlogPostDto>, AddBlogPostDtoValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
