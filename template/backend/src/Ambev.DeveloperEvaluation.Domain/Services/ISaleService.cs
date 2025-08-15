using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service interface for Sale business operations.
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale.</returns>
    Task<Sale> CreateSaleAsync(Sale sale, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale.</returns>
    Task<Sale> UpdateSaleAsync(Sale sale, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Cancels a sale.
    /// </summary>
    /// <param name="saleId">The ID of the sale to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cancelled sale.</returns>
    Task<Sale> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Cancels an item in a sale.
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item.</param>
    /// <param name="itemId">The ID of the item to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale with the cancelled item.</returns>
    Task<Sale> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a sale by its sale number.
    /// </summary>
    /// <param name="saleNumber">The sale number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetSaleByNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedSalesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales within a date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales within the date range.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the customer.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByCustomerAsync(
        Guid customerId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales for a specific branch.
    /// </summary>
    /// <param name="branchId">The ID of the branch.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the branch.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesByBranchAsync(
        Guid branchId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}