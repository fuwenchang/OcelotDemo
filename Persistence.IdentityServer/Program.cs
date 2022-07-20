using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.IdentityServer;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();


//Identity
builder.Services.AddDbContext<AspNetAccountDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetAccountDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // ÃÜÂë¸´ÔÓ¶ÈÅäÖÃ
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});



var migrationsAssembly = typeof(Config).Assembly.GetName().Name;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddConfigurationStore(options =>
{
    options.ConfigureDbContext = builder =>
    {
        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
    };
})
        .AddOperationalStore(options =>
{
    options.ConfigureDbContext = builder =>
    {
        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
    };
})
        .AddAspNetIdentity<ApplicationUser>();

//×Ô¶¯Ç¨ÒÆ£¬²¥ÖÖÊý¾Ý
//SeedData.EnsureSeedData(connectionString);
//SeedData.EnsureSeedAspNetAccountData(connectionString);//Identity


// ÅäÖÃcookie²ßÂÔ
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
});

var app = builder.Build();
app.UseStaticFiles();
app.UseIdentityServer();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
