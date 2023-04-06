using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository.Seed;

public class ProductFeatureSeed:IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {
        builder.HasData(
            new ProductFeature { Id = 1, Color = "Kırmızı", Height = 150, Width = 12, ProductId = 1 },
            new ProductFeature { Id = 2, Color = "Siyah", Height = 125, Width = 16, ProductId = 2 }
            );
    }
}