using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.IdentityServer
{

    public class AspNetAccountDbContext : IdentityDbContext<ApplicationUser>
    {
        public AspNetAccountDbContext(DbContextOptions<AspNetAccountDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    public class ApplicationUser : IdentityUser
    { }
}
