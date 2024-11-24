using App.AuthAPI.Services;
using App.Data.DbContexts;
using App.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<ApiJwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<AuthApiDbContext>();
context.Database.EnsureCreated();

app.Run();
