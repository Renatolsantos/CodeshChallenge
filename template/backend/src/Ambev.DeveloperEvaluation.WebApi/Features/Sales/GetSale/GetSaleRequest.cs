﻿namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Request model for retrieving a sale.
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// Gets or sets the ID of the sale to retrieve.
    /// </summary>
    public Guid Id { get; set; }
}