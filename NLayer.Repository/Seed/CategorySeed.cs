using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository.Seed;

public class CategorySeed:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(
            new Category { Id = 1, Name = "Pencils" },
            new Category { Id = 2, Name = "Books" },
            new Category { Id = 3, Name = "Notebooks" });    }
}