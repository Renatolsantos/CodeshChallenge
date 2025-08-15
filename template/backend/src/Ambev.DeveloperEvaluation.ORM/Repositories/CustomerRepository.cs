using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICustomerRepository using Entity Framework Core.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CustomerRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The customer if found, null otherwise.</returns>
    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets a customer by their external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The customer if found, null otherwise.</returns>
    public async Task<Customer?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.ExternalId == externalId, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of customers.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of customers per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of customers.</returns>
    public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPaginatedListAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Customers.CountAsync(cancellationToken);
        
        var customers = await _context.Customers
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (customers, totalCount);
    }

    /// <summary>
    /// Creates or updates a customer with data from an external system.
    /// </summary>
    /// <param name="customer">The customer data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated customer.</returns>
    public async Task<Customer> UpsertFromExternalAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ExternalId == customer.ExternalId, cancellationToken);
            
        if (existingCustomer != null)
        {
            // Update existing customer
            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.LastSyncedAt = DateTime.UtcNow;
            
            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync(cancellationToken);
            return existingCustomer;
        }
        else
        {
            // Create new customer
            customer.LastSyncedAt = DateTime.UtcNow;
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }
    }
}