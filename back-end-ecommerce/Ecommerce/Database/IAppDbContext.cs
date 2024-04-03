using Microsoft.EntityFrameworkCore;
public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Product> Products { get; set; }

    int SaveChanges();
}