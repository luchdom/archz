using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.SharedKernel.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra;

public sealed class AppWriteDbContext(
    DbContextOptions<AppWriteDbContext> options,
    IPublisher? publisher = null)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Count != 0)
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        if (publisher is not null)
        {
            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppWriteDbContext).Assembly,
            WriteConfigurationsFilter);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("EntityConfigurations.Write") ?? false;
}
