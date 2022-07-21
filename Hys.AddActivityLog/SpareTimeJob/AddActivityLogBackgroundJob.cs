using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSRedis;

using Furion.DatabaseAccessor;
using Furion.TaskScheduler;

using Hys.AddActivityLog.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hys.AddActivityLog.SpareTimeJob
{
    public class AddActivityLogBackgroundJob : BackgroundService
    {
        private readonly ILogger<AddActivityLogBackgroundJob> _logger;
        private readonly IActivityService _activityService;
        private CSRedisClient _csredis;
        public AddActivityLogBackgroundJob(
            ILogger<AddActivityLogBackgroundJob> logger,
            IActivityService activityService,
            CSRedisClient csredis)
        {
            _logger = logger;
            _activityService = activityService;
            _csredis = csredis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SpareTime.Do("0/5 * * * * ?", (timer, count) =>
            {
                Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Console.WriteLine($"一共执行了：{count} 次");

                _activityService.AddActivityDaily().Wait();

            }, cronFormat: CronFormat.IncludeSeconds);
        }        
    }
}
