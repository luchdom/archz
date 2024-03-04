using Archz.SharedKernel.SeedWork;

namespace Archz.Products.Api.Domain.AggregateModels.ProductAggregates;

public sealed record ProductCreatedEvent(Product Product) : IDomainEvent;
