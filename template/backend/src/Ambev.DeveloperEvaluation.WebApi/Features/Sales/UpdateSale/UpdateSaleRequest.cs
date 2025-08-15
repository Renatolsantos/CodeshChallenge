using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request model for updating a sale.
/// </summary>
public class UpdateSaleRequest
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
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Request model for sale items.
/// </summary>
public class UpdateSaleItemRequest
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