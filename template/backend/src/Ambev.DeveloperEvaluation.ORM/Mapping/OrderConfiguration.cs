using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class OrderConfiguration : BaseEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Orders");

        // OrderNumber configuration
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        // OrderDate configuration
        builder.Property(o => o.OrderDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // CustomerRefId configuration
        builder.Property(o => o.CustomerRefId)
            .IsRequired()
            .HasColumnType("uuid");

        // BranchRefId configuration
        builder.Property(o => o.BranchRefId)
            .IsRequired()
            .HasColumnType("uuid");

        // TotalAmount configuration
        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Status configuration
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // CartRefId configuration
        builder.Property(o => o.CartRefId)
            .IsRequired()
            .HasColumnType("uuid");

        // Configure relationship with OrderItems
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure indexes
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();

        builder.HasIndex(o => o.CustomerRefId);

        builder.HasIndex(o => o.BranchRefId);

        builder.HasIndex(o => o.CartRefId);

        builder.HasIndex(o => o.OrderDate);

        builder.HasIndex(o => o.Status);
    }
}
