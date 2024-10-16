using App.AdminMVC.Services;
using App.Services;
using App.Services.AdminServices.Abstract;

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

var authApiUrl = builder.Configuration.GetValue<string>("AuthApiUrl");

if (string.IsNullOrWhiteSpace(authApiUrl))
{
    throw new InvalidOperationException("AuthApiUrl is required in appsettings.json");
}

builder.Services.AddHttpClient("authApi", c =>
{
    c.BaseAddress = new Uri(authApiUrl);
});

builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IPersonalInfoService, PersonalInfoService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();

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
