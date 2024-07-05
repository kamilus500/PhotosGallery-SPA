using PhotosGallerySPA.Domain.Configs;
using PhotosGallerySPA.Infrastructure.Extensions;
using PhotosGallerySPA.Infrastructure.Hubs;
using PhotosGallerySPA.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);
builder.Services.AddControllersWithViews();

builder.Services.Configure<SmtpSettings>(settings => configuration.GetSection("SmtpSettings").Bind(settings));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<AccessHub>("/accessDenied");
});

app.Run();
