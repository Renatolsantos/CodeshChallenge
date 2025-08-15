using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for the Customer entity.
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Configures the entity mapping for the Customer entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.ExternalId)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(255);
               
        builder.Property(c => c.Email)
               .HasMaxLength(255);
               
        builder.Property(c => c.Phone)
               .HasMaxLength(20);
               
        builder.Property(c => c.LastSyncedAt)
               .IsRequired();
               
        // Add a unique index on ExternalId
        builder.HasIndex(c => c.ExternalId).IsUnique();
    }
}