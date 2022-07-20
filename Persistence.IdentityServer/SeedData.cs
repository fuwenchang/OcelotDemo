using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Persistence.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var migtationAssembly = typeof(SeedData).Assembly.GetName().Name;
            var service = new ServiceCollection();
            service.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migtationAssembly));
            });
            service.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migtationAssembly));
            });
            service.AddDbContext<AspNetAccountDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            var serviceProvider = service.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //临时
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
                //配置
                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                context.Database.Migrate();
                EnsureSeedData(context);
                
                    
            }
        }

        private static void EnsureSeedData(IConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                    context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }
            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.Apis)
                    context.ApiResources.Add(api.ToEntity());
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var id in Config.IdentityResources)
                    context.IdentityResources.Add(id.ToEntity());
                context.SaveChanges();
            }
        }

        public static void EnsureSeedAspNetAccountData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AspNetAccountDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetAccountDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<AspNetAccountDbContext>();
                    context.Database.Migrate();

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var user = userManager.FindByNameAsync("fan").Result;
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = "fan",
                            Email = "410577910@qq.com",
                            
                        };
                        var result = userManager.CreateAsync(user, "123456").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        result = userManager.AddClaimsAsync(user, new Claim[] {
                        new Claim(JwtClaimTypes.Name, "fan"),
                        new Claim(JwtClaimTypes.GivenName, "fan"),
                        new Claim(JwtClaimTypes.FamilyName, "fan"),
                        new Claim(JwtClaimTypes.Email, "410577910@qq.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://fan.com"),
                        new Claim(JwtClaimTypes.Address, @"{ '城市': '北京', '邮政编码': '10000' }",
                            IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    // 为了代码简单一点,删除其他用户数据
                }
            }
        }
    }
}
