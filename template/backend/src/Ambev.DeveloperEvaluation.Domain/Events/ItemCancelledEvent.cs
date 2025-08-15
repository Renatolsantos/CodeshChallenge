using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when an item within a sale is cancelled.
/// </summary>
public class ItemCancelledEvent : INotification
{
    /// <summary>
    /// Gets the sale containing the cancelled item.
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the item that was cancelled.
    /// </summary>
    public SaleItem Item { get; }

    /// <summary>
    /// Initializes a new instance of ItemCancelledEvent.
    /// </summary>
    /// <param name="sale">The sale containing the cancelled item.</param>
    /// <param name="item">The cancelled item.</param>
    public ItemCancelledEvent(Sale sale, SaleItem item)
    {
        Sale = sale;
        Item = item;
    }
}