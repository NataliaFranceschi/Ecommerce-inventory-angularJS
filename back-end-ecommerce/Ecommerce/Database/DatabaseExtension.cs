using Microsoft.EntityFrameworkCore;

public static class DatabaseExtension
{
    public static void ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(options => options
        .UseSqlite(connectionString));

        services.AddDbContext<IAppDbContext, AppDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

    }

}
