using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for the Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the entity mapping for the Product entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.ExternalId)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(255);
               
        builder.Property(p => p.Description)
               .HasMaxLength(1000);
               
        builder.Property(p => p.Price)
               .IsRequired()
               .HasPrecision(18, 2);
               
        builder.Property(p => p.LastSyncedAt)
               .IsRequired();
               
        // Add a unique index on ExternalId
        builder.HasIndex(p => p.ExternalId).IsUnique();
    }
}