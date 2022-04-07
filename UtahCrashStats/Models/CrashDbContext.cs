using System;
using Microsoft.EntityFrameworkCore;

namespace UtahCrashStats.Models
{
    public class CrashDbContext : DbContext
    {
        public CrashDbContext(DbContextOptions<CrashDbContext> options) : base(options)
        {

        }

        public DbSet<Crash> Crash { get; set; }
    }
}
