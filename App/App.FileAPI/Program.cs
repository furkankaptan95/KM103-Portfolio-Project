using App.FileAPI;
using App.Services.AuthService.Abstract;
using App.Services.AuthService.Concrete;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // T�m origin'lere izin verir.
               .AllowAnyMethod() // T�m HTTP y�ntemlerine izin verir.
               .AllowAnyHeader(); // T�m ba�l�klara izin verir.
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

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

builder.Services.AddScoped<IAuthService, AuthService>();

JwtAuthMethod.AddJwtAuth(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
