using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {
            if (opt is null)
            {
                throw new ArgumentNullException(nameof(opt));
            }
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}