using Furion;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api_Auth
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<RefreshTokenSettingOptions>();

            services.AddJwt<JwtHandler>();
            services.AddCorsAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorsAccessor();
        }
    }
}
