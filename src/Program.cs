using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RTMProxy;
using RTMProxy.Models;
using RTMProxy.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RTMProxy", Version = "v1" });
});

builder.Services.Configure<ProxySettings>(builder.Configuration.GetSection("ProxySettings"));

builder.Services.AddSingleton<INginxService, NginxService>();
builder.Services.AddSingleton<IProxyConfigService, ProxyConfigService>(x =>
{
    var options = x.GetService<IOptions<ProxySettings>>()
    ?? throw new InvalidProgramException("Cannot read proxy settings from config");

    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    return new ProxyConfigService(options.Value.ConfigFile, jsonOptions);
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var myService = scope.ServiceProvider.GetRequiredService<IProxyConfigService>();
    _ = await myService.GetAsync(CancellationToken.None);
}

app.Run();
