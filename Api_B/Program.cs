using Hys.AddActivityLog;

var builder = WebApplication.CreateBuilder(args).Inject();
var configBuild = builder.Configuration.AddJsonFile("appsettings.Development.json");
var configuration = configBuild.Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 注入定时任务
builder.Services.AddActivity(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRequestResponseLogging();
app.MapControllers();

app.Run();
