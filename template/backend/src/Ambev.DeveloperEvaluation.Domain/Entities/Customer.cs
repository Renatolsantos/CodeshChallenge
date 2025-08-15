using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer in the system.
/// This is an external entity referenced using the External Identities pattern.
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// Gets or sets the external ID of the customer.
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email of the customer.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the customer was last synced from the external system.
    /// </summary>
    public DateTime LastSyncedAt { get; set; }
}