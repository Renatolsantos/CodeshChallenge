using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler for processing ItemCancelledEvent.
/// </summary>
public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ItemCancelledEventHandler.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the ItemCancelledEvent notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;
        var item = notification.Item;
        
        // Log the event (in a real application, this would publish to a message broker)
        _logger.LogInformation(
            "Item cancelled event: Sale ID {SaleId}, Sale Number {SaleNumber}, Item ID {ItemId}, " +
            "Product {ProductName}, Quantity {Quantity}, Unit Price {UnitPrice}, Discount {DiscountPercentage}%, " +
            "Total Price {TotalPrice}",
            sale.Id,
            sale.SaleNumber,
            item.Id,
            item.Product.Name,
            item.Quantity,
            item.UnitPrice,
            item.DiscountPercentage * 100,
            item.TotalPrice);
            
        return Task.CompletedTask;
    }
}