using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a branch/store in the system.
/// This is an external entity referenced using the External Identities pattern.
/// </summary>
public class Branch : BaseEntity
{
    /// <summary>
    /// Gets or sets the external ID of the branch.
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the branch.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the branch was last synced from the external system.
    /// </summary>
    public DateTime LastSyncedAt { get; set; }
}