using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is modified.
/// </summary>
public class SaleModifiedEvent : INotification
{
    /// <summary>
    /// Gets the sale that was modified.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of SaleModifiedEvent.
    /// </summary>
    /// <param name="sale">The modified sale.</param>
    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }
}