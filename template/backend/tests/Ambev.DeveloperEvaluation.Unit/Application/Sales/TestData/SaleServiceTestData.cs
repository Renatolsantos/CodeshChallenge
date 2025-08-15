using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Test data for SaleService tests.
/// </summary>
public static class SaleServiceTestData
{
    /// <summary>
    /// Gets a test sale with items.
    /// </summary>
    /// <returns>A sale instance for testing.</returns>
    public static Sale GetTestSale()
    {
        return SaleTestData.GetTestSale();
    }
    
    /// <summary>
    /// Gets a list of test sales.
    /// </summary>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A list of sales for testing.</returns>
    public static List<Sale> GetTestSales(int count = 5)
    {
        var sales = new List<Sale>();
        
        for (int i = 1; i <= count; i++)
        {
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = $"SALE-{i:000}",
                SaleDate = DateTime.UtcNow.AddDays(-i),
                Customer = SaleTestData.GetTestCustomer(),
                Branch = SaleTestData.GetTestBranch()
            };
            
            var product = SaleItemTestData.GetTestProduct(100m * i);
            sale.AddItem(new SaleItem(product, i % 20 + 1)); // Quantity between 1 and 20
            
            sales.Add(sale);
        }
        
        return sales;
    }
}