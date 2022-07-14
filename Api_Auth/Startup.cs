using Furion;

namespace Api_Auth
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<RefreshTokenSettingOptions>();
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
            services.AddCorsAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorsAccessor();
        }
    }
}
