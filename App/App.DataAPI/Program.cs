using Microsoft.EntityFrameworkCore;
using App.Data.DbContexts;
using App.Services.AdminServices.Abstract;
using App.DataAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataApiBaseDb"));
});

builder.Services.AddScoped<IAboutMeService, AboutMeService>();

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
context.Database.EnsureCreated();

app.Run();
