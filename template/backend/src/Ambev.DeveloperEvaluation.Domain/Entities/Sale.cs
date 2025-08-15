using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales record in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer associated with this sale.
    /// </summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// </summary>
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// Gets or sets the items included in this sale.
    /// </summary>
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date when the sale was created in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date when the sale was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount of the sale based on its items.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(item => item.TotalPrice);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(SaleItem item)
    {
        Items.Add(item);
        CalculateTotalAmount();
    }

    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public void RemoveItem(SaleItem item)
    {
        Items.Remove(item);
        CalculateTotalAmount();
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the sale.
    /// </summary>
    /// <param name="itemId">The ID of the item to cancel.</param>
    /// <returns>True if the item was found and cancelled, false otherwise.</returns>
    public bool CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            return false;

        item.Cancel();
        CalculateTotalAmount();
        return true;
    }
}