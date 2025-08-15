using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Test data for SaleItem entity tests.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Gets a test product.
    /// </summary>
    /// <param name="price">The price of the product.</param>
    /// <returns>A product instance for testing.</returns>
    public static Product GetTestProduct(decimal price = 100m)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Product Description",
            Price = price,
            ExternalId = "EXT-001",
            LastSyncedAt = DateTime.UtcNow
        };
    }
    
    /// <summary>
    /// Gets test data for discount business rules.
    /// </summary>
    /// <returns>A collection of test cases for discount business rules.</returns>
    public static IEnumerable<object[]> GetDiscountTestData()
    {
        // Case 1: Less than 4 items - No discount
        yield return new object[] { 3, 100m, 0m, 300m };
        
        // Case 2: 4 items - 10% discount
        yield return new object[] { 4, 100m, 0.1m, 360m };
        
        // Case 3: 9 items - 10% discount
        yield return new object[] { 9, 100m, 0.1m, 810m };
        
        // Case 4: 10 items - 20% discount
        yield return new object[] { 10, 100m, 0.2m, 800m };
        
        // Case 5: 15 items - 20% discount
        yield return new object[] { 15, 100m, 0.2m, 1200m };
        
        // Case 6: 20 items - 20% discount (maximum allowed)
        yield return new object[] { 20, 100m, 0.2m, 1600m };
    }
    
    /// <summary>
    /// Gets test data for quantity validation.
    /// </summary>
    /// <returns>A collection of test cases for quantity validation.</returns>
    public static IEnumerable<object[]> GetInvalidQuantityTestData()
    {
        // Case 1: Zero quantity
        yield return new object[] { 0, "Quantity must be greater than zero." };
        
        // Case 2: Negative quantity
        yield return new object[] { -5, "Quantity must be greater than zero." };
        
        // Case 3: Quantity exceeding maximum limit
        yield return new object[] { 21, "Cannot sell more than 20 identical items." };
        yield return new object[] { 50, "Cannot sell more than 20 identical items." };
    }
}