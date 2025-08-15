﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving a sale by its ID.
/// </summary>
public class GetSaleCommand : IRequest<GetSaleResult?>
{
    /// <summary>
    /// Gets or sets the ID of the sale to retrieve.
    /// </summary>
    public Guid Id { get; set; }
}