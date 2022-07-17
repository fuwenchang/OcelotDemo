using Hys.Framework.Consul;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

var configurationBuild = builder.Configuration.
    AddJsonFile("appsettings.json").
    AddJsonFile("configuration.json");
var configuration = configurationBuild.Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});

//builder.Services.AddConsul(configuration);

builder.Services.AddOcelot().AddConsul();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("ÎÒÊÇOcelotÍø¹Ø!");
    });
});
app.UseOcelot().Wait();
app.MapControllers();

app.Run();
