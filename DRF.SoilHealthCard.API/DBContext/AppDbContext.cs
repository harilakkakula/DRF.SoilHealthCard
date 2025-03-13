using DRF.SoilHealthCard.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DRF.SoilHealthCard.API.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
