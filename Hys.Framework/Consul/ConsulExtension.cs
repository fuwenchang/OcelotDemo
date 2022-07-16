using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hys.Framework.Consul
{
    public static class ConsulExtension
    {
        /// <summary>
        /// webapi服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConsul(this IServiceCollection services, IConfigurationRoot configuration)
        {
            ConsulClient consulClient = new ConsulClient(
                x => x.Address = new Uri("http://localhost:8500/")
                );

            /* 这种获取ip的方式，需要在命令行中输入ip的值
                如：dotnet Api_A.dll --urls http://localhost:5002 --ip 127.0.0.1 --port 5002
                下面的port一样
            */

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                Timeout = TimeSpan.FromSeconds(5),
                HTTP = $"http://{ip}:{port}/HealthCheck/Status",
                
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = configuration["Consul:ServiceName"],
                Address = ip,
                Port = port,
                Tags = new[] { $"urlprefix-/{configuration["Consul:Tags"]}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

            services.AddSingleton(consulClient);
        }

        /// <summary>
        /// grpc服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddGrpcConsul(this IServiceCollection services, IConfigurationRoot configuration)
        {
            ConsulClient consulClient = new ConsulClient(
                x => x.Address = new Uri(configuration["Consul:ConsulAddress"])
                );

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                Timeout = TimeSpan.FromSeconds(5),
                GRPC = $"{configuration["Consul:ServerAddress"]}:{configuration["Consul:ServciePort"]}",//健康检查地址
                GRPCUseTLS = false
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = configuration["Consul:ServiceName"],
                Address = configuration["Consul:ServerAddress"],
                Port = int.Parse(configuration["Consul:ServciePort"]),
                Tags = new[] { $"urlprefix-/{configuration["Consul:Tags"]}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

            services.AddSingleton(consulClient);
        }

        /// <summary>
        /// 健康检查 - 仅支持webapi，grpc没用
        /// </summary>
        /// <param name="builder"></param>
        //public static void UseConsul(this IApplicationBuilder builder)
        //{
        //    builder.Map("/status", r =>
        //    {
        //        r.Run(async handler =>
        //        {
        //            handler.Response.StatusCode = 200;
        //            await handler.Response.WriteAsync("ok");
        //        });
        //    });
        //}
    }
}
