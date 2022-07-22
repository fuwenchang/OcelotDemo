using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace Hys.Framework.CustomLog
{
    public static class LogConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logEventLevel">将哪个级别的日志生成日志文件,Error已经默认生成</param>
        /// <returns></returns>
        public static WebApplicationBuilder UseSerilogConfig(this WebApplicationBuilder builder, LogEventLevel logEventLevel = LogEventLevel.Error)
        {
            builder.UseSerilogDefault(config =>//默认集成了 控制台 和 文件 方式。如需自定义写入，则传入需要写入的介质即可：
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");//按时间创建文件夹
                string outputTemplate = "{NewLine}【{Level:u3}】{Timestamp:yyyy-MM-dd HH:mm:ss.fff}" +
                "{NewLine}#Msg#{Message:lj}" +
                "{NewLine}#Pro #{Properties:j}" +
                "{NewLine}#Exc#{Exception}" +
                new string('-', 50);//输出模板

                config.WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                       .WriteTo.File($"_log/{date}/{LogEventLevel.Error}.log",
                           outputTemplate: outputTemplate,
                           rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
                           encoding: Encoding.UTF8            // 文件字符编码
                        )
                    );

                if (logEventLevel == LogEventLevel.Debug)
                {
                    config.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                       .WriteTo.File($"_log/{date}/{LogEventLevel.Debug}.log",
                           outputTemplate: outputTemplate,
                           rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
                           encoding: Encoding.UTF8            // 文件字符编码
                        )
                    );
                }
                else if (logEventLevel == LogEventLevel.Information)
                {
                    config.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                       .WriteTo.File($"_log/{date}/{LogEventLevel.Information}.log",
                           outputTemplate: outputTemplate,
                           rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
                           encoding: Encoding.UTF8            // 文件字符编码
                        )
                    );
                }
            });

            return builder;
        }

        #region 已注释：完整的配置可参考
        //public static WebApplicationBuilder UseSerilogConfig(this WebApplicationBuilder builder, LogEventLevel logLevel)
        //{
        //    builder.UseSerilogDefault(config =>//默认集成了 控制台 和 文件 方式。如需自定义写入，则传入需要写入的介质即可：
        //    {
        //        string date = DateTime.Now.ToString("yyyy-MM-dd");//按时间创建文件夹
        //        string outputTemplate = "{NewLine}【{Level:u3}】{Timestamp:yyyy-MM-dd HH:mm:ss.fff}" +
        //        "{NewLine}#Msg#{Message:lj}" +
        //        "{NewLine}#Pro #{Properties:j}" +
        //        "{NewLine}#Exc#{Exception}" +
        //        new string('-', 50);//输出模板

        //        config
        //        #region 1、输出所有日志
        //           ////.MinimumLevel.Debug() // 所有Sink的最小记录级别
        //           ////.MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
        //           ////.Enrich.FromLogContext()
        //           //.WriteTo.Console(outputTemplate: outputTemplate)
        //           //.WriteTo.File($"_log/{date}/application.log",
        //           //       outputTemplate: outputTemplate,
        //           //        restrictedToMinimumLevel: LogEventLevel.Information,
        //           //        rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
        //           //                                             //rollOnFileSizeLimit: true,          // 限制单个文件的最大长度
        //           //                                             //retainedFileCountLimit: 10,         // 最大保存文件数,等于null时永远保留文件。
        //           //                                             //fileSizeLimitBytes: 10 * 1024,      // 最大单个文件大小
        //           //        encoding: Encoding.UTF8            // 文件字符编码
        //           //    )
        //        #endregion

        //        #region 2.按LogEventLevel.输出独立发布/单文件
        //           ///2.1仅输出 LogEventLevel.Debug 类型
        //           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug)//筛选过滤
        //               .WriteTo.File($"_log/{date}/{LogEventLevel.Debug}.log",
        //                   outputTemplate: outputTemplate,
        //                   rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
        //                   encoding: Encoding.UTF8            // 文件字符编码
        //                )
        //            )
        //           ///2.2仅输出 LogEventLevel.Error 类型
        //           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)//筛选过滤
        //               .WriteTo.File($"_log/{date}/{LogEventLevel.Error}.log",
        //                   outputTemplate: outputTemplate,
        //                   rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
        //                   encoding: Encoding.UTF8            // 文件字符编码
        //                )
        //            )
        //        #endregion 按LogEventLevel 独立发布/单文件
        //            ;
        //    });

        //    return builder;
        //}
        #endregion
    }
}
