using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for the SaleItem entity.
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the entity mapping for the SaleItem entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.SaleId)
               .IsRequired();
               
        builder.Property(i => i.Quantity)
               .IsRequired();
               
        builder.Property(i => i.UnitPrice)
               .IsRequired()
               .HasPrecision(18, 2);
               
        builder.Property(i => i.DiscountPercentage)
               .IsRequired()
               .HasPrecision(5, 4);
               
        builder.Property(i => i.TotalPrice)
               .IsRequired()
               .HasPrecision(18, 2);
               
        builder.Property(i => i.IsCancelled)
               .IsRequired();
        
        // Configure the relationship with Product
        builder.HasOne(i => i.Product)
               .WithMany()
               .HasForeignKey("ProductId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}