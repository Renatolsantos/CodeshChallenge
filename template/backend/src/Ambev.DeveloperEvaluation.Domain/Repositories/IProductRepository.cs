using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product if found, null otherwise.</returns>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a product by its external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product if found, null otherwise.</returns>
    Task<Product?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of products.</returns>
    Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates or updates a product with data from an external system.
    /// </summary>
    /// <param name="product">The product data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated product.</returns>
    Task<Product> UpsertFromExternalAsync(Product product, CancellationToken cancellationToken = default);
}