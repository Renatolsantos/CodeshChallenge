using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale.</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number.
    /// </summary>
    /// <param name="saleNumber">The sale number to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale in the database.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale.</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Entry(sale).State = EntityState.Modified;
        
        // For each item in the sale, update it if it exists or add it if it's new
        foreach (var item in sale.Items)
        {
            var existingItem = await _context.SaleItems
                .FirstOrDefaultAsync(si => si.Id == item.Id, cancellationToken);
                
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(item);
            }
            else
            {
                await _context.SaleItems.AddAsync(item, cancellationToken);
            }
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Gets a paginated list of sales.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Sales.CountAsync(cancellationToken);
        
        var sales = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (sales, totalCount);
    }

    /// <summary>
    /// Gets a paginated list of sales for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the customer.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByCustomerAsync(
        Guid customerId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Sales
            .CountAsync(s => s.Customer.Id == customerId, cancellationToken);
        
        var sales = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.Customer.Id == customerId)
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (sales, totalCount);
    }

    /// <summary>
    /// Gets a paginated list of sales for a specific branch.
    /// </summary>
    /// <param name="branchId">The ID of the branch.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of sales per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sales for the branch.</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByBranchAsync(
        Guid branchId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Sales
            .CountAsync(s => s.Branch.Id == branchId, cancellationToken);
        
        var sales = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.Branch.Id == branchId)
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (sales, totalCount);
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
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetPaginatedListByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Sales
            .CountAsync(s => s.SaleDate >= startDate && s.SaleDate <= endDate, cancellationToken);
        
        var sales = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (sales, totalCount);
    }
}