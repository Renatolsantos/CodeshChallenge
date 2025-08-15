using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for the Branch entity.
/// </summary>
public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    /// <summary>
    /// Configures the entity mapping for the Branch entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.ExternalId)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(b => b.Name)
               .IsRequired()
               .HasMaxLength(255);
               
        builder.Property(b => b.Address)
               .HasMaxLength(500);
               
        builder.Property(b => b.LastSyncedAt)
               .IsRequired();
               
        // Add a unique index on ExternalId
        builder.HasIndex(b => b.ExternalId).IsUnique();
    }
}