using Archz.Products.Api.Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra;

public sealed class AppReadDbContext(DbContextOptions<AppReadDbContext> options)
    : DbContext(options)
{
    public DbSet<ProductReadModel> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppReadDbContext).Assembly,
            ReadConfigurationsFilter);
    }

    private static bool ReadConfigurationsFilter(Type type) =>
        type.FullName?.Contains("EntityConfigurations.Read") ?? false;
}
