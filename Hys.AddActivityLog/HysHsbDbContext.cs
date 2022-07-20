using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hys.AddActivityLog.Models;

using Microsoft.EntityFrameworkCore;

namespace Hys.AddActivityLog
{
    public class HysHsbDbContext : DbContext
    {
        public HysHsbDbContext(DbContextOptions<HysHsbDbContext> options)
            : base(options)
        {
        }
        public DbSet<ActivityDaily>? ActivityDailies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityDaily>().ToTable("ActivityDaily");
        }
    }
}
