using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra;

internal sealed class AppReadDbContext(DbContextOptions<AppReadDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppReadDbContext).Assembly,
            ReadConfigurationsFilter);
    }

    private static bool ReadConfigurationsFilter(Type type) =>
        type.FullName?.Contains("EntityConfigurations.Read") ?? false;
}
