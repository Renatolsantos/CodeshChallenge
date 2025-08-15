using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Test data for Sale entity tests.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Gets a test customer.
    /// </summary>
    /// <returns>A customer instance for testing.</returns>
    public static Customer GetTestCustomer()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Email = "customer@test.com",
            Phone = "123-456-7890",
            ExternalId = "CUST-001",
            LastSyncedAt = DateTime.UtcNow
        };
    }
    
    /// <summary>
    /// Gets a test branch.
    /// </summary>
    /// <returns>A branch instance for testing.</returns>
    public static Branch GetTestBranch()
    {
        return new Branch
        {
            Id = Guid.NewGuid(),
            Name = "Test Branch",
            Address = "123 Test Street",
            ExternalId = "BRANCH-001",
            LastSyncedAt = DateTime.UtcNow
        };
    }
    
    /// <summary>
    /// Gets a test sale with items.
    /// </summary>
    /// <returns>A sale instance for testing.</returns>
    public static Sale GetTestSale()
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SALE-001",
            SaleDate = DateTime.UtcNow,
            Customer = GetTestCustomer(),
            Branch = GetTestBranch()
        };
        
        // Add items
        var product1 = SaleItemTestData.GetTestProduct(100m);
        var product2 = SaleItemTestData.GetTestProduct(200m);
        
        sale.AddItem(new SaleItem(product1, 5)); // 10% discount, total: 450
        sale.AddItem(new SaleItem(product2, 3)); // No discount, total: 600

        sale.Items = sale.Items.Select(i => { i.Id = Guid.NewGuid(); return i; }).ToList();

        return sale;
    }
}