using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler for processing SaleCreatedEvent.
/// </summary>
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCreatedEventHandler.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleCreatedEvent notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;
        
        // Log the event (in a real application, this would publish to a message broker)
        _logger.LogInformation(
            "Sale created event: Sale ID {SaleId}, Sale Number {SaleNumber}, Customer {CustomerName}, " +
            "Branch {BranchName}, Total Amount {TotalAmount}, Items Count {ItemsCount}, Date {SaleDate}",
            sale.Id,
            sale.SaleNumber,
            sale.Customer.Name,
            sale.Branch.Name,
            sale.TotalAmount,
            sale.Items.Count,
            sale.SaleDate);
            
        return Task.CompletedTask;
    }
}