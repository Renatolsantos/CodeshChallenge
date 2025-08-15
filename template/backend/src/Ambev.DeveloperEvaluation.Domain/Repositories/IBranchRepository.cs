using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations.
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Gets a branch by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a branch by its external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    Task<Branch?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of branches.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of branches per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of branches.</returns>
    Task<(IEnumerable<Branch> Branches, int TotalCount)> GetPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates or updates a branch with data from an external system.
    /// </summary>
    /// <param name="branch">The branch data from the external system.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created or updated branch.</returns>
    Task<Branch> UpsertFromExternalAsync(Branch branch, CancellationToken cancellationToken = default);
}