using System;
using Microsoft.EntityFrameworkCore;

namespace UtahCrashStats.Models
{
    public class StoryDbContext : DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options)
        {

        }

        public DbSet<Story> Story { get; set; }
    }
}
