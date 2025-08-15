﻿using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response model for created sale.
/// </summary>
public class CreateSaleResponse
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
    public CreateCustomerResponseDto Customer { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the branch information.
    /// </summary>
    public CreateBranchResponseDto Branch { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the items in the sale.
    /// </summary>
    public List<CreateSaleItemDetailResponseDto> Items { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// DTO for customer information in the response.
/// </summary>
public class CreateCustomerResponseDto
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
/// DTO for branch information in the response.
/// </summary>
public class CreateBranchResponseDto
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
/// DTO for sale item details in the response.
/// </summary>
public class CreateSaleItemDetailResponseDto
{
    /// <summary>
    /// Gets or sets the item ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product information.
    /// </summary>
    public CreateProductResponseDto Product { get; set; } = null!;
    
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
}

/// <summary>
/// DTO for product information in the response.
/// </summary>
public class CreateProductResponseDto
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