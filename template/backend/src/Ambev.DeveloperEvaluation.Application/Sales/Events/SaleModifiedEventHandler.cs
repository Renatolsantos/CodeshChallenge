using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler for processing SaleModifiedEvent.
/// </summary>
public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleModifiedEventHandler.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleModifiedEvent notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;
        
        // Log the event (in a real application, this would publish to a message broker)
        _logger.LogInformation(
            "Sale modified event: Sale ID {SaleId}, Sale Number {SaleNumber}, Customer {CustomerName}, " +
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