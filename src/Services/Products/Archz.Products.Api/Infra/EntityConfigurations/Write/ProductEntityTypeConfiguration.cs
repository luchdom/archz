using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra.EntityConfigurations.Write;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);
        builder.Property(o => o.Id)
            .UseHiLo("productseq");

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.IsActive)
           .IsRequired()
           .HasDefaultValue(false);

    }
}
