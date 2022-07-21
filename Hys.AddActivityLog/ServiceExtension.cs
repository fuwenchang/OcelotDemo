
using Furion.DatabaseAccessor;

using Hys.AddActivityLog.Middleware;
using Hys.AddActivityLog.SpareTimeJob;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hys.AddActivityLog
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddActivity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HysHsbDbContext>(
                options =>
                options.UseSqlServer(configuration["ConnectionStrings:HysHsbConnectionString"])
                , ServiceLifetime.Singleton);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<IActivityService, ActivityService>();
            // 注入定时任务
            services.AddHostedService<AddActivityLogBackgroundJob>();

            // 注入redis
            var rds = new CSRedis.CSRedisClient(configuration["Redis:RedisStr"]);
            services.AddSingleton(rds);

            return services;
        }

        /// <summary>
        /// 全局异常 和 日志记录
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionLogMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware(typeof(CustomerExceptionMiddleware));
        }
    }
}
