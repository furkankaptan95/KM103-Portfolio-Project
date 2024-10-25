using App.Middlewares;
using App.PortfolioMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        response.Redirect("/Auth/Login");
    }
    else if (response.StatusCode == StatusCodes.Status403Forbidden)
    {
        response.Redirect("/Home/AccessDenied");
    }
    return Task.CompletedTask;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
