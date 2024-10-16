using App.AuthAPI.Services;
using App.Data.DbContexts;
using App.Services.AdminServices.Abstract;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7071") // Ýzin verilen köken
               .AllowAnyMethod() // Ýzin verilen HTTP yöntemleri
               .AllowAnyHeader(); // Ýzin verilen baþlýklar
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataApiUrl = builder.Configuration.GetValue<string>("DataApiUrl");

if (string.IsNullOrWhiteSpace(dataApiUrl))
{
    throw new InvalidOperationException("DataApiUrl is required in appsettings.json");
}

builder.Services.AddHttpClient("dataApi", c =>
{
    c.BaseAddress = new Uri(dataApiUrl);
});


builder.Services.AddDbContext<AuthApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthApiBaseDb"));
});

builder.Services.AddScoped<IUserService, AdminUserService>();

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
using var context = scope.ServiceProvider.GetRequiredService<AuthApiDbContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

app.Run();
