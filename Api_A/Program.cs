using System.Text.RegularExpressions;
using Hys.Framework.Consul;

var builder = WebApplication.CreateBuilder(args);

var configBuild = builder.Configuration.AddJsonFile("appsettings.Development.json");
var configuration = configBuild.Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ½¡¿µ¼ì²é
//app.UseConsul();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
