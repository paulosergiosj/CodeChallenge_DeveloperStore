using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : BaseEntityConfiguration<Product>, IEntityTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("Products");

        builder.Property(p => p.ProductNumber)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Category)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.ImageUrl)
               .HasMaxLength(500);

        builder.OwnsOne(p => p.Rating);
    }
}
