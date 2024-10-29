using App.Core.Authorization;
using App.Middlewares;
using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7241","https://localhost:7167") // Ýzin verilen köken
               .AllowAnyMethod() // Ýzin verilen HTTP yöntemleri
               .AllowAnyHeader(); // Ýzin verilen baþlýklar
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthorizationService>();
var authApiUrl = builder.Configuration.GetValue<string>("AuthApiUrl");
if (string.IsNullOrWhiteSpace(authApiUrl))
{
    throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
}
builder.Services.AddHttpClient("authApi", c =>
{
    c.BaseAddress = new Uri(authApiUrl);
});

builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});

app.UseHttpsRedirection();

app.UseMiddleware<ApiJwtMiddleware>();

app.UseAuthorization();

app.MapControllers();



app.Run();
