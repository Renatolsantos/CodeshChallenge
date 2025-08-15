using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Customer entity operations.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Gets a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The customer if found, null otherwise.</returns>
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a customer by their external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The customer if found, null otherwise.</returns>
    Task<Customer?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of customers.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of customers per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of customers.</returns>
    Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates or updates a customer with data from an external system.
    /// </summary>
    /// <param name="customer">The customer data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated customer.</returns>
    Task<Customer> UpsertFromExternalAsync(Customer customer, CancellationToken cancellationToken = default);
}