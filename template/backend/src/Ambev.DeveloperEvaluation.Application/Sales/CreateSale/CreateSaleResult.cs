using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Result returned after creating a sale.
/// </summary>
public class CreateSaleResult
{
    /// <summary>
    /// Gets or sets the ID of the created sale.
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
    /// Gets or sets the customer information.
    /// </summary>
    public CreateCustomerResultDto Customer { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the branch information.
    /// </summary>
    public CreateBranchResultDto Branch { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the items in the sale.
    /// </summary>
    public List<CreateSaleItemDetailResultDto> Items { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets whether the sale is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for customer information in the result.
/// </summary>
public class CreateCustomerResultDto
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer's email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// DTO for branch information in the result.
/// </summary>
public class CreateBranchResultDto
{
    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch address.
    /// </summary>
    public string Address { get; set; } = string.Empty;
}

/// <summary>
/// DTO for sale item details in the result.
/// </summary>
public class CreateSaleItemDetailResultDto
{
    /// <summary>
    /// Gets or sets the item ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product information.
    /// </summary>
    public CreateProductResultDto Product { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the discount percentage.
    /// </summary>
    public decimal DiscountPercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the total price.
    /// </summary>
    public decimal TotalPrice { get; set; }
    
    /// <summary>
    /// Gets or sets whether the item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// DTO for product information in the result.
/// </summary>
public class CreateProductResultDto
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }
}