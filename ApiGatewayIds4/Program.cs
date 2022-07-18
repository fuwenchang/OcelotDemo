using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
var configurationBuild = builder.Configuration.
    AddJsonFile("configuration.json", optional: false, reloadOnChange: true).
    Build();
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});
builder.Services.AddOcelot().AddConsul();



var app = builder.Build();
app.UseOcelot().Wait();
app.Run();
