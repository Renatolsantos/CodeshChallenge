﻿namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Result returned after cancelling a sale.
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// Gets or sets the ID of the cancelled sale.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the sale is successfully cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount of the sale before cancellation.
    /// </summary>
    public decimal TotalAmount { get; set; }
}