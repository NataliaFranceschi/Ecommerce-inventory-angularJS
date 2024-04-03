using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Seeder.SeedUserAdmin(this);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

}
