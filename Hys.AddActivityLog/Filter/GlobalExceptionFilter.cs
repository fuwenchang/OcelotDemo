using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSRedis;

using Hys.AddActivityLog.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Hys.AddActivityLog.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private ActivityDaily activityDaily;
        private CSRedisClient _csredis;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(
            CSRedisClient csredis,
            ILogger<GlobalExceptionFilter> logger)
        {
            _csredis = csredis;
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 初始化日志实体
            activityDaily = InitActivityDailyEntity(context.HttpContext);
            activityDaily.ServiceEnd = DateTime.Now;
            activityDaily.Duration = activityDaily.ServiceEnd.Subtract(activityDaily.ServiceStart).Milliseconds;

            if (context.Exception is BusinessException)
            {
                var ex = (BusinessException)context.Exception;

                activityDaily.Status = 1;
                activityDaily.CallStatus = 1;
                activityDaily.Output = JsonConvert.SerializeObject(CommonResult.Failed<string>(ex.Message));
                
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(CommonResult.Failed<string>(ex.Message))
                };
            }
            else 
            {                
                activityDaily.Status = -1;
                activityDaily.CallStatus = -1;
                activityDaily.Output = JsonConvert.SerializeObject(CommonResult.Failed<string>(context.Exception.Message));

                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(CommonResult.Failed<string>("程序异常"))
                };
            }

            _logger.LogInformation($"VisitLog: {Newtonsoft.Json.JsonConvert.SerializeObject(activityDaily)}");

            // 放入redis
            _csredis.LPush(RedisKey.ActivityDaily, activityDaily);

            // 设置为true，表示异常已经被处理了
            context.ExceptionHandled = true;
            return Task.CompletedTask;
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
    }
}
