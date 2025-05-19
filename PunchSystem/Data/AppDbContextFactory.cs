using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Services;

namespace PunchSystem.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Make sure to read from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // path to appsettings.json
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("Default"));

            // Use a dummy implementation that returns static values
            var dummyUserContext = new DummyUserContextService();

            return new AppDbContext(optionsBuilder.Options, dummyUserContext);
        }
    }

    // Dummy user context for EF CLI
    public class DummyUserContextService : IUserContextService
    {
        public string? GetCurrentUserId() => "system";
        public string? GetCurrentUsername() => "system";
    }
}
