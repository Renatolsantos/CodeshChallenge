using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Tests for the Sale entity.
/// </summary>
public class SaleTests
{
    [Fact]
    public void Sale_CalculateTotalAmount_SumsTotalPriceOfAllItems()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        
        // Act & Assert
        Assert.Equal(1050m, sale.TotalAmount); // 450 + 600 = 1050
    }
    
    [Fact]
    public void Sale_AddItem_RecalculatesTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        var originalTotal = sale.TotalAmount;
        var product = SaleItemTestData.GetTestProduct(300m);
        var newItem = new SaleItem(product, 2); // No discount, total: 600
        
        // Act
        sale.AddItem(newItem);
        
        // Assert
        Assert.Equal(originalTotal + 600m, sale.TotalAmount); // 1050 + 600 = 1650
    }
    
    [Fact]
    public void Sale_RemoveItem_RecalculatesTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        var itemToRemove = sale.Items.First();
        var itemTotal = itemToRemove.TotalPrice;
        var originalTotal = sale.TotalAmount;
        
        // Act
        sale.RemoveItem(itemToRemove);
        
        // Assert
        Assert.Equal(originalTotal - itemTotal, sale.TotalAmount);
        Assert.DoesNotContain(itemToRemove, sale.Items);
    }
    
    [Fact]
    public void Sale_Cancel_SetsIsCancelledToTrue()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        
        // Act
        sale.Cancel();
        
        // Assert
        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }
    
    [Fact]
    public void Sale_CancelItem_ReturnsTrueAndRecalculatesTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        var itemToCancel = sale.Items.First();
        var itemId = itemToCancel.Id;
        var originalTotal = sale.TotalAmount;
        var itemTotal = itemToCancel.TotalPrice;
        
        // Act
        var result = sale.CancelItem(itemId);
        
        // Assert
        Assert.True(result);
        Assert.True(itemToCancel.IsCancelled);
        Assert.Equal(originalTotal - itemTotal, sale.TotalAmount);
    }
    
    [Fact]
    public void Sale_CancelItem_ReturnsFalseForNonexistentItem()
    {
        // Arrange
        var sale = SaleTestData.GetTestSale();
        var nonExistentItemId = Guid.NewGuid();
        var originalTotal = sale.TotalAmount;
        
        // Act
        var result = sale.CancelItem(nonExistentItemId);
        
        // Assert
        Assert.False(result);
        Assert.Equal(originalTotal, sale.TotalAmount); // Total remains unchanged
    }
}