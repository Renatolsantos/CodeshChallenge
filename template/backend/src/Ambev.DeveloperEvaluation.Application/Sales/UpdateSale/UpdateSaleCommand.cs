using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets or sets the ID of the sale to update.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public Guid BranchId { get; set; }
    
    /// <summary>
    /// Gets or sets the list of items in the sale.
    /// </summary>
    public List<SaleItemDto> Items { get; set; } = new();
}

/// <summary>
/// Data transfer object for sale items in the command.
/// </summary>
public class SaleItemDto
{
    /// <summary>
    /// Gets or sets the item ID. Can be null for new items.
    /// </summary>
    public Guid? Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }
}