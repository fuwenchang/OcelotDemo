using System.Text.RegularExpressions;
using Hys.Framework.Consul;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configBuild = builder.Configuration.AddJsonFile("appsettings.Development.json");
var configuration = configBuild.Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("ApiGateway", new OpenApiInfo { Title = "Íø¹Ø·þÎñ", Version = "v1" });
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddConsul(configuration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
