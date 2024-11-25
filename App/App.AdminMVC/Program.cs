using App.AdminMVC.Services;
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

app.UseAuthentication();
app.UseAuthorization();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
app.Use(async (context, next) =>
{
    // CSRF token yaln�zca POST, PUT, DELETE gibi hassas isteklerde do�rulan�r
    if (HttpMethods.IsPost(context.Request.Method) ||
        HttpMethods.IsPut(context.Request.Method) ||
        HttpMethods.IsDelete(context.Request.Method))
    {
        // Token do�rulamas�n� yap�yoruz
        await antiforgery.ValidateRequestAsync(context);
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
