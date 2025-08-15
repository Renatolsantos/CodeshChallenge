using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a new sale is created.
/// </summary>
public class SaleCreatedEvent : INotification
{
    /// <summary>
    /// Gets the sale that was created.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of SaleCreatedEvent.
    /// </summary>
    /// <param name="sale">The newly created sale.</param>
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}