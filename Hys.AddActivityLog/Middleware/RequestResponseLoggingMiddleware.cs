using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSRedis;

using Hys.AddActivityLog.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Hys.AddActivityLog.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private RequestResponseLog _logInfo;
        private ActivityDaily activityDaily;
        private CSRedisClient _csredis;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger,
            CSRedisClient csredis)
        {
            _next = next;
            _logger = logger;
            _csredis = csredis;
        }

        

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            // 健康检查 不需要记录日志
            if (request.Path.ToString().ToLower().Contains("healthcheck/status"))
            {
                await _next(context);
                return;
            }

            string accountId = context.User?.Claims?.SingleOrDefault(a => a.Type == "Account")?.Value ?? string.Empty;
            int userId = int.Parse(context.User?.Claims?.SingleOrDefault(a => a.Type == "UserId")?.Value ?? "0");
            string name = context.User?.Claims?.SingleOrDefault(a => a.Type == "Name")?.Value ?? "";

            activityDaily = new ActivityDaily()
            {
                CreatedTime = DateTime.Now,
                CreatedUserId = userId,
                CreatedUserName = name,
                UpdatedTime = DateTime.Now,
                UpdatedUserId = userId,
                UpdatedUserName = name,
                IsDeleted = false,
                AccountId = accountId
            };

            
            activityDaily.ServerHostAddress = context.Connection.LocalIpAddress?.MapToIPv4()?.ToString();
            activityDaily.UserHostAddress = request.Host.Host;
            activityDaily.ServiceStart = DateTime.Now;

            // 获取请求参数
            GetRequestInput(request, activityDaily);

            // 获取Response.Body内容
            await GetResponseBody(context, activityDaily);

            _logger.LogInformation($"VisitLog: {JsonConvert.SerializeObject(activityDaily)}");

            // 放入redis
            _csredis.LPush(RedisKey.ActivityDaily, activityDaily);
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void GetRequestInput(HttpRequest request, ActivityDaily activityDaily)
        {
            //获取request.Body内容
            if (request.Method.ToLower().Equals("post"))
            {
                //request.EnableRewind(); //启用倒带功能，就可以让 Request.Body 可以再次读取，.net 5弃用
                request.EnableBuffering();

                Stream stream = request.Body;
                byte[] buffer = new byte[request.ContentLength.Value];
                stream.ReadAsync(buffer, 0, buffer.Length);
                activityDaily.Input = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;

            }
            else if (request.Method.ToLower().Equals("get"))
            {
                activityDaily.Input = request.QueryString.Value ?? "";
            }
        }

        /// <summary>
        /// 获取Response.Body内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task GetResponseBody(HttpContext context, ActivityDaily activityDaily)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                activityDaily.Output = await FormatResponse(context.Response);
                activityDaily.ServiceEnd = DateTime.Now;
                TimeSpan date_poor = activityDaily.ServiceEnd.Subtract(activityDaily.ServiceStart);
                activityDaily.Duration = date_poor.Milliseconds;
                activityDaily.Status = 1;
                
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}
