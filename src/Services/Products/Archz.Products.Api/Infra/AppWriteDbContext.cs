using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.SharedKernel.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra;

public sealed class AppWriteDbContext(DbContextOptions<AppWriteDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppWriteDbContext).Assembly,
            WriteConfigurationsFilter);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("EntityConfigurations.Write") ?? false;
}
