namespace Archz.Products.Api.Infra.Models;

public sealed record ProductReadModel(
    int Id,
    string Name,
    decimal Price,
    bool IsActive);
