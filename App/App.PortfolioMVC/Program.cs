using App.Middlewares;
using App.PortfolioMVC.Services;
using Microsoft.AspNetCore.Antiforgery;

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

app.UseMiddleware<MvcJwtMiddleware>();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
app.Use(async (context, next) =>
{
    // CSRF token yalnýzca POST, PUT, DELETE gibi hassas isteklerde doðrulanýr
    if (HttpMethods.IsPost(context.Request.Method) ||
        HttpMethods.IsPut(context.Request.Method) ||
        HttpMethods.IsDelete(context.Request.Method))
    {
        // Token doðrulamasýný yapýyoruz
        await antiforgery.ValidateRequestAsync(context);
    }
    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index2}/{id?}");

app.Run();
