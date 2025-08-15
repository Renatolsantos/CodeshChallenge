using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sales domain operations.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the database.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale.</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a sale by its sale number.
    /// </summary>
    /// <param name="saleNumber">The sale number to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing sale in the database.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale.</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the customer.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByCustomerAsync(Guid customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales for a specific branch.
    /// </summary>
    /// <param name="branchId">The ID of the branch.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the branch.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByBranchAsync(Guid branchId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of sales within a date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales within the date range.</returns>
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}