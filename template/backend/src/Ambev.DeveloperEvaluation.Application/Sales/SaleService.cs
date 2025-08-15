using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales;

/// <summary>
/// Implementation of ISaleService that handles the business logic for sales operations.
/// </summary>
public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<SaleService> _logger;

    /// <summary>
    /// Initializes a new instance of SaleService.
    /// </summary>
    /// <param name="saleRepository">The sale repository.</param>
    /// <param name="mediator">The mediator for publishing events.</param>
    /// <param name="logger">The logger.</param>
    public SaleService(
        ISaleRepository saleRepository,
        IMediator mediator,
        ILogger<SaleService> logger)
    {
        _saleRepository = saleRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale.</returns>
    public async Task<Sale> CreateSaleAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        // Calculate the total amount
        sale.CalculateTotalAmount();
        
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        
        // Publish event
        await PublishSaleCreatedEventAsync(createdSale, cancellationToken);
        
        return createdSale;
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale.</returns>
    public async Task<Sale> UpdateSaleAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingSale = await _saleRepository.GetByIdAsync(sale.Id, cancellationToken);
        if (existingSale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {sale.Id} not found.");
        }
        
        // Recalculate the total amount
        sale.CalculateTotalAmount();
        
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish event
        await PublishSaleModifiedEventAsync(updatedSale, cancellationToken);
        
        return updatedSale;
    }

    /// <summary>
    /// Cancels a sale.
    /// </summary>
    /// <param name="saleId">The ID of the sale to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cancelled sale.</returns>
    public async Task<Sale> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
        }
        
        // Cancel the sale
        sale.Cancel();
        
        var cancelledSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish event
        await PublishSaleCancelledEventAsync(cancelledSale, cancellationToken);
        
        return cancelledSale;
    }

    /// <summary>
    /// Cancels an item in a sale.
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item.</param>
    /// <param name="itemId">The ID of the item to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale with the cancelled item.</returns>
    public async Task<Sale> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
        }
        
        var item = sale.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new KeyNotFoundException($"Item with ID {itemId} not found in sale {saleId}.");
        }
        
        item.Cancel();
        
        // Recalculate the total amount
        sale.CalculateTotalAmount();
        
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish event
        await PublishItemCancelledEventAsync(updatedSale, item, cancellationToken);
        
        return updatedSale;
    }

    /// <summary>
    /// Gets a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    public async Task<Sale?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetByIdAsync(id, cancellationToken);
    }

    /// <summary>
    /// Gets a sale by its sale number.
    /// </summary>
    /// <param name="saleNumber">The sale number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    public async Task<Sale?> GetSaleByNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetBySaleNumberAsync(saleNumber, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of sales.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedSalesAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of sales within a date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales within the date range.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetPaginatedListByDateRangeAsync(startDate, endDate, pageNumber, pageSize, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of sales for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the customer.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByCustomerAsync(
        Guid customerId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetPaginatedListByCustomerAsync(customerId, pageNumber, pageSize, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of sales for a specific branch.
    /// </summary>
    /// <param name="branchId">The ID of the branch.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the branch.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByBranchAsync(
        Guid branchId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _saleRepository.GetPaginatedListByBranchAsync(branchId, pageNumber, pageSize, cancellationToken);
    }
    
    #region Private Methods
    
    private async Task PublishSaleCreatedEventAsync(Sale sale, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new SaleCreatedEvent(sale);
            await _mediator.Publish(@event, cancellationToken);
            _logger.LogInformation("SaleCreatedEvent published for sale {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing SaleCreatedEvent for sale {SaleId}", sale.Id);
        }
    }
    
    private async Task PublishSaleModifiedEventAsync(Sale sale, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new SaleModifiedEvent(sale);
            await _mediator.Publish(@event, cancellationToken);
            _logger.LogInformation("SaleModifiedEvent published for sale {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing SaleModifiedEvent for sale {SaleId}", sale.Id);
        }
    }
    
    private async Task PublishSaleCancelledEventAsync(Sale sale, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new SaleCancelledEvent(sale);
            await _mediator.Publish(@event, cancellationToken);
            _logger.LogInformation("SaleCancelledEvent published for sale {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing SaleCancelledEvent for sale {SaleId}", sale.Id);
        }
    }
    
    private async Task PublishItemCancelledEventAsync(Sale sale, SaleItem item, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new ItemCancelledEvent(sale, item);
            await _mediator.Publish(@event, cancellationToken);
            _logger.LogInformation("ItemCancelledEvent published for item {ItemId} in sale {SaleId}", item.Id, sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing ItemCancelledEvent for item {ItemId} in sale {SaleId}", item.Id, sale.Id);
        }
    }
    
    #endregion
}