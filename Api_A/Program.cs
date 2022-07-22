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

// ����־��¼���ļ���
builder.UseSerilogConfig(Serilog.Events.LogEventLevel.Debug);


// �ӹ�������Ϊ�˿��Ǻ����ڹ�������������ȷ����Ȩ
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(SimpleActionFilter));
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

#region ͨ��Jwt��������Ȩ
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //ȡ��˽Կ
        var secretByte = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            //��֤������
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            //��֤������
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            //��֤�Ƿ����
            ValidateLifetime = true,
            //��֤˽Կ
            IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        };
    });

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// ע�붨ʱ����
builder.Services.AddActivity(configuration);


var app = builder.Build();
app.UseHttpsRedirection();
// �Դ���http������־��¼
//app.UseHttpLogging();
app.AddConsul(configuration);
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionLogMiddleware();
app.MapControllers();
app.Run();
