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
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is BusinessException)
            {
                var ex = (BusinessException)context.Exception;
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(CommonResult.Failed<string>(ex.Message))
                };
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(CommonResult.Failed<string>("程序异常"))
                };
            }

            // 设置为true，表示异常已经被处理了
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
