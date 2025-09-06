// Infrastructure/Persistence/FinanzasContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AppFinanzas.Infrastructure.Persistence
{
    public class FinanzasContextFactory : IDesignTimeDbContextFactory<FinanzasContext>
    {
        public FinanzasContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = cfg.GetConnectionString("Default") ?? cfg["ConnectionStrings:Default"];
            var opt = new DbContextOptionsBuilder<FinanzasContext>()
                .UseSqlServer(cs)
                .Options;

            return new FinanzasContext(opt);
        }
    }
}
