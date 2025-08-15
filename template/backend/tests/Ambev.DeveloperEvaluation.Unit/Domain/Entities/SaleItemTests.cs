using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Tests for the SaleItem entity.
/// </summary>
public class SaleItemTests
{
    [Theory]
    [MemberData(nameof(SaleItemTestData.GetDiscountTestData), MemberType = typeof(SaleItemTestData))]
    public void SaleItem_AppliesCorrectDiscountBasedOnQuantity(
        int quantity, decimal unitPrice, decimal expectedDiscountPercentage, decimal expectedTotalPrice)
    {
        // Arrange
        var product = SaleItemTestData.GetTestProduct(unitPrice);
        
        // Act
        var saleItem = new SaleItem(product, quantity);
        
        // Assert
        Assert.Equal(expectedDiscountPercentage, saleItem.DiscountPercentage);
        Assert.Equal(expectedTotalPrice, saleItem.TotalPrice);
    }
    
    [Theory]
    [MemberData(nameof(SaleItemTestData.GetInvalidQuantityTestData), MemberType = typeof(SaleItemTestData))]
    public void SaleItem_ThrowsExceptionForInvalidQuantity(int invalidQuantity, string expectedExceptionMessage)
    {
        // Arrange
        var product = SaleItemTestData.GetTestProduct();
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new SaleItem(product, invalidQuantity));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
    
    [Fact]
    public void SaleItem_UpdateQuantity_UpdatesDiscountAndTotalPrice()
    {
        // Arrange
        var product = SaleItemTestData.GetTestProduct(100m);
        var saleItem = new SaleItem(product, 3); // Initially no discount
        
        // Act
        saleItem.UpdateQuantity(10); // Should get 20% discount
        
        // Assert
        Assert.Equal(10, saleItem.Quantity);
        Assert.Equal(0.2m, saleItem.DiscountPercentage);
        Assert.Equal(800m, saleItem.TotalPrice); // 10 * 100 * 0.8 = 800
    }
    
    [Fact]
    public void SaleItem_Cancel_SetsTotalPriceToZero()
    {
        // Arrange
        var product = SaleItemTestData.GetTestProduct(100m);
        var saleItem = new SaleItem(product, 5); // 10% discount
        var originalTotal = saleItem.TotalPrice;
        
        // Act
        saleItem.Cancel();
        
        // Assert
        Assert.True(saleItem.IsCancelled);
        Assert.Equal(0m, saleItem.TotalPrice);
        Assert.NotEqual(originalTotal, saleItem.TotalPrice);
    }
}