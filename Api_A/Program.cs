using System.Text;

using Api_A.Filter;

using Hys.AddActivityLog;
using Hys.AddActivityLog.Filter;
using Hys.Framework.Consul;
using Hys.Framework.CustomLog;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args).Inject();
var configBuild = builder.Configuration.AddJsonFile("appsettings.Development.json",false, reloadOnChange: true);
var configuration = configBuild.Build();

// 将日志记录到文件中
builder.UseSerilogConfig(Serilog.Events.LogEventLevel.Debug);


// 加过滤器是为了考虑后面在过滤器中做更精确的授权
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(SimpleActionFilter));
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

#region 通过Jwt来进行授权
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //取出私钥
        var secretByte = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            //验证发布者
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            //验证接收者
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            //验证是否过期
            ValidateLifetime = true,
            //验证私钥
            IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        };
    });

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// 注入定时任务
builder.Services.AddActivity(configuration);


var app = builder.Build();
app.UseHttpsRedirection();
// 自带的http请求日志记录
//app.UseHttpLogging();
app.AddConsul(configuration);
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionLogMiddleware();
app.MapControllers();
app.Run();
