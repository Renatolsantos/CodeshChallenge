using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core.
/// </summary>
public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of BranchRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BranchRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a branch by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets a branch by its external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    public async Task<Branch?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .FirstOrDefaultAsync(b => b.ExternalId == externalId, cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of branches.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of branches per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of branches.</returns>
    public async Task<(IEnumerable<Branch> Branches, int TotalCount)> GetPaginatedListAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Branches.CountAsync(cancellationToken);
        
        var branches = await _context.Branches
            .OrderBy(b => b.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (branches, totalCount);
    }

    /// <summary>
    /// Creates or updates a branch with data from an external system.
    /// </summary>
    /// <param name="branch">The branch data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated branch.</returns>
    public async Task<Branch> UpsertFromExternalAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        var existingBranch = await _context.Branches
            .FirstOrDefaultAsync(b => b.ExternalId == branch.ExternalId, cancellationToken);
            
        if (existingBranch != null)
        {
            // Update existing branch
            existingBranch.Name = branch.Name;
            existingBranch.Address = branch.Address;
            existingBranch.LastSyncedAt = DateTime.UtcNow;
            
            _context.Branches.Update(existingBranch);
            await _context.SaveChangesAsync(cancellationToken);
            return existingBranch;
        }
        else
        {
            // Create new branch
            branch.LastSyncedAt = DateTime.UtcNow;
            await _context.Branches.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }
    }
}