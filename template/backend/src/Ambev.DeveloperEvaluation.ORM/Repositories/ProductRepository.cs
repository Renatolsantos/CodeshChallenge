using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product if found, null otherwise.</returns>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets a product by its external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product if found, null otherwise.</returns>
    public async Task<Product?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.ExternalId == externalId, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of products.</returns>
    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedListAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Products.CountAsync(cancellationToken);
        
        var products = await _context.Products
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (products, totalCount);
    }

    /// <summary>
    /// Creates or updates a product with data from an external system.
    /// </summary>
    /// <param name="product">The product data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated product.</returns>
    public async Task<Product> UpsertFromExternalAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.ExternalId == product.ExternalId, cancellationToken);
            
        if (existingProduct != null)
        {
            // Update existing product
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.LastSyncedAt = DateTime.UtcNow;
            
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync(cancellationToken);
            return existingProduct;
        }
        else
        {
            // Create new product
            product.LastSyncedAt = DateTime.UtcNow;
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
    }
}