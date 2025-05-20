using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PunchSystem.Services;

namespace PunchSystem.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            // Tu peux passer null si tu ne veux pas injecter le service utilisateur à la main
            return new AppDbContext(optionsBuilder.Options, new FakeUserContextService());
        }
    }

    public class FakeUserContextService : IUserContextService
    {
        public string? GetCurrentUserId() => "SYSTEM";
        public string? GetCurrentUsername() => "SYSTEM";
    }
}
