using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<Program> where TProgram : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(dbContextDescriptor);
            var dbConnetionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            services.Remove(dbConnetionDescriptor);
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<AppDbContext>((container, option) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                option.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}
