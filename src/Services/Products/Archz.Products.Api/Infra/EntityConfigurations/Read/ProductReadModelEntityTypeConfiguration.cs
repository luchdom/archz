using Archz.Products.Api.Infra.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Archz.Products.Api.Infra.EntityConfigurations.Read;

public sealed class ProductReadModelEntityTypeConfiguration
    : IEntityTypeConfiguration<ProductReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(product => product.Price)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(product => product.IsActive)
            .IsRequired();
    }
}
