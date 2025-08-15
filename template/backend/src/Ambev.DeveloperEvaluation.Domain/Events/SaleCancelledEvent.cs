using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent : INotification
{
    /// <summary>
    /// Gets the sale that was cancelled.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of SaleCancelledEvent.
    /// </summary>
    /// <param name="sale">The cancelled sale.</param>
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }
}