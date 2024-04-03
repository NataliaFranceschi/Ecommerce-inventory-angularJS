using Microsoft.EntityFrameworkCore;

public static class MigrationExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        context?.Database.Migrate();
    }
}