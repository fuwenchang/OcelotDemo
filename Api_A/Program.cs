using System.Text;
using System.Text.RegularExpressions;
using Api_A.Filter;
using Hys.Framework.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configBuild = builder.Configuration.AddJsonFile("appsettings.Development.json");
var configuration = configBuild.Build();

#region ͨ��Jwt��������Ȩ
// �ӹ�������Ϊ�˿��Ǻ����ڹ�������������ȷ����Ȩ
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(SimpleActionFilter));
});
builder.Services.AddSingleton<SimpleActionFilter>();

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



var app = builder.Build();
app.UseHttpsRedirection();
app.AddConsul(configuration);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
