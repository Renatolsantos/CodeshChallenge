using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for the Sale entity.
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the entity mapping for the Sale entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.SaleNumber)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(s => s.SaleDate)
               .IsRequired();
               
        builder.Property(s => s.TotalAmount)
               .IsRequired()
               .HasPrecision(18, 2);
               
        builder.Property(s => s.IsCancelled)
               .IsRequired();
               
        builder.Property(s => s.CreatedAt)
               .IsRequired();
               
        builder.Property(s => s.UpdatedAt);
        
        // Configure the relationship with Customer
        builder.HasOne(s => s.Customer)
               .WithMany()
               .HasForeignKey("CustomerId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
               
        // Configure the relationship with Branch
        builder.HasOne(s => s.Branch)
               .WithMany()
               .HasForeignKey("BranchId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
               
        // Configure the relationship with SaleItems
        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey("SaleId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
               
        // Add a unique index on SaleNumber
        builder.HasIndex(s => s.SaleNumber).IsUnique();
    }
}