using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrderItems");

        // OrderId configuration
        builder.Property(oi => oi.OrderId)
            .IsRequired()
            .HasColumnType("uuid");

        // ProductRefId configuration
        builder.Property(oi => oi.ProductRefId)
            .IsRequired()
            .HasColumnType("uuid");

        // ProductRefNumber configuration
        builder.Property(oi => oi.ProductRefNumber)
            .IsRequired();

        // UnitPrice configuration
        builder.Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Quantity configuration
        builder.Property(oi => oi.Quantity)
            .IsRequired();

        // DiscountAmount configuration
        builder.Property(oi => oi.DiscountAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // TotalAmount configuration
        builder.Property(oi => oi.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Configure indexes
        builder.HasIndex(oi => oi.OrderId);

        builder.HasIndex(oi => oi.ProductRefId);

        builder.HasIndex(oi => oi.ProductRefNumber);

        // Composite index for better query performance
        builder.HasIndex(oi => new { oi.OrderId, oi.ProductRefNumber })
            .IsUnique();

        // Configure foreign key constraints to prevent deletion of referenced entities
        // Note: These are logical constraints since we're using reference IDs
        // The actual enforcement will be done in the application layer
        
        // Configure cascade delete behavior
        builder.HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of orders with items
    }
}
