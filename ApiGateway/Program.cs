
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args).Inject();
var configurationBuild = builder.Configuration.AddJsonFile("appsettings.json");
var configuration = configurationBuild.Build();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});
builder.Services.AddOcelot(new ConfigurationBuilder()
  .AddJsonFile("configuration.json")
  .Build());





var app = builder.Build();
app.UseOcelot().Wait();
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
