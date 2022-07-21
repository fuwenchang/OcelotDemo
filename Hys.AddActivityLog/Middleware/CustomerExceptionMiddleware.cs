using System.Text;
using System.Text.Json;

using CSRedis;

using Hys.AddActivityLog.Filter;
using Hys.AddActivityLog.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hys.AddActivityLog.Middleware
{
    /// <summary>
    /// 自定义异常中间件
    /// </summary>
    public class CustomerExceptionMiddleware
    {
        /// <summary>
        /// 委托
        /// </summary>
        private readonly RequestDelegate _next;
        private ActivityDaily activityDaily;
        private CSRedisClient _csredis;
        private readonly ILogger<CustomerExceptionMiddleware> _logger;

        public CustomerExceptionMiddleware(
            RequestDelegate next,
            ILogger<CustomerExceptionMiddleware> logger,
            CSRedisClient csredis)
        {
            _next = next;
            _logger = logger;
            _csredis = csredis;
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    var request = context.Request;
        //    // 健康检查 不需要记录日志
        //    if (request.Path.ToString().ToLower().Contains("healthcheck/status"))
        //    {
        //        await _next(context);
        //        return;
        //    }

        //    try
        //    {
        //        // 初始化日志实体
        //        activityDaily = InitActivityDailyEntity(context);

        //        // 获取请求参数
        //        GetRequestInput(request, activityDaily);

        //        await _next(context);

        //        // 获取Response.Body内容
        //        await GetResponseBody(context, activityDaily);

        //    }
        //    catch (Exception ex)
        //    {
        //        activityDaily.ServiceEnd = DateTime.Now;
        //        TimeSpan date_poor = activityDaily.ServiceEnd.Subtract(activityDaily.ServiceStart);
        //        activityDaily.Duration = date_poor.Milliseconds;


        //        context.Response.ContentType = "application/json;charset=utf-8";
        //        var stream = context.Response.Body;
        //        if (ex is BusinessException)
        //        {
        //            var exBusiness = (BusinessException)ex;
        //            activityDaily.Status = 2;
        //            activityDaily.CallStatus = 2;
        //            activityDaily.Output = Newtonsoft.Json.JsonConvert.SerializeObject(CommonResult.Failed<string>(exBusiness.Message));

        //            await JsonSerializer.SerializeAsync(stream, CommonResult.Failed<string>(exBusiness.Message));
        //        }
        //        else
        //        {
        //            activityDaily.Status = -1;
        //            activityDaily.CallStatus = -1;
        //            activityDaily.Output = Newtonsoft.Json.JsonConvert.SerializeObject(CommonResult.Failed<string>(ex.Message));

        //            await JsonSerializer.SerializeAsync(stream, CommonResult.Failed<string>("程序异常"));
        //        }
        //    }

        //    _logger.LogInformation($"VisitLog: {Newtonsoft.Json.JsonConvert.SerializeObject(activityDaily)}");

        //    // 放入redis
        //    _csredis.LPush(RedisKey.ActivityDaily, activityDaily);
        //}

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            // 健康检查 不需要记录日志
            if (request.Path.ToString().ToLower().Contains("healthcheck/status"))
            {
                await _next(context);
                return;
            }

            // 初始化日志实体
            activityDaily = InitActivityDailyEntity(context);

            // 获取请求参数
            GetRequestInput(request, activityDaily);

            // 获取Response.Body内容
            await GetResponseBody(context, activityDaily);
        }

        private ActivityDaily InitActivityDailyEntity(HttpContext context)
        {
            #region TODO:id自增，到时候看怎么修改
            if (string.IsNullOrEmpty(_csredis.Get(RedisKey.ActivityDailyId)))
            {
                _csredis.Set(RedisKey.ActivityDailyId, 142303330922122);
            }
            _csredis.IncrBy(RedisKey.ActivityDailyId);
            #endregion


            string accountId = context.User?.Claims?.SingleOrDefault(a => a.Type == "Account")?.Value ?? string.Empty;
            int userId = int.Parse(context.User?.Claims?.SingleOrDefault(a => a.Type == "UserId")?.Value ?? "0");
            string name = context.User?.Claims?.SingleOrDefault(a => a.Type == "Name")?.Value ?? "";

            ActivityDaily activityDaily = new ActivityDaily()
            {
                Id = _csredis.Get<long>(RedisKey.ActivityDailyId),
                ServiceId = "ceshifuwu",    // TODO:服务id
                InterfaceId = "ceshijiekou",// TODO:接口id
                CreatedTime = DateTime.Now,
                CreatedUserId = userId,
                CreatedUserName = name,
                UpdatedTime = DateTime.Now,
                UpdatedUserId = userId,
                UpdatedUserName = name,
                IsDeleted = false,
                AccountId = accountId,
                ServerHostAddress = context.Connection.LocalIpAddress?.MapToIPv4()?.ToString(),
                UserHostAddress = context.Request.Host.Host,
                ServiceStart = DateTime.Now
            };

            return activityDaily;
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

                // 过滤器捕获到异常后，执行到这里，如果不加判断，日志会被里面的字段给代替掉
                if (context.Response.StatusCode == 200)
                {
                    activityDaily.Output = await FormatResponse(context.Response);
                    activityDaily.ServiceEnd = DateTime.Now;
                    activityDaily.Duration = activityDaily.ServiceEnd.Subtract(activityDaily.ServiceStart).Milliseconds;
                    activityDaily.Status = 1;
                    activityDaily.CallStatus = 1;

                    _logger.LogInformation($"VisitLog: {Newtonsoft.Json.JsonConvert.SerializeObject(activityDaily)}");

                    // 放入redis
                    _csredis.LPush(RedisKey.ActivityDaily, activityDaily);

                    await responseBody.CopyToAsync(originalBodyStream);
                }             
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
