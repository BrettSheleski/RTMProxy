using Microsoft.Extensions.Options;
using RTMProxy;
using RTMProxy.Models;
using RTMProxy.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ProxySettings>(builder.Configuration.GetSection("ProxySettings"));


builder.Services.AddSingleton<IProxyConfigService, ProxyConfigService>(x =>
{
    var options = x.GetService<IOptions<ProxySettings>>()
    ?? throw new InvalidProgramException("Cannot read proxy settings from config");

    var configFile = new FileInfo(options.Value.ConfigFile);

    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    return new ProxyConfigService(configFile, jsonOptions);
});
builder.Services.AddScoped<HomeIndexViewModel>();



var app = builder.Build();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var myService = scope.ServiceProvider.GetRequiredService<IProxyConfigService>();
    _ = await myService.GetAsync(CancellationToken.None);
}

app.Run();
