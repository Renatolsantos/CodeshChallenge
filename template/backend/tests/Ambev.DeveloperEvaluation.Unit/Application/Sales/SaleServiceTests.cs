using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Tests for the SaleService class.
/// </summary>
public class SaleServiceTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<SaleService> _logger;
    private readonly SaleService _saleService;
    
    public SaleServiceTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<SaleService>>();
        
        _saleService = new SaleService(_saleRepository, _mediator, _logger);
    }
    
    [Fact]
    public async Task CreateSaleAsync_CalculatesTotalAmountAndSavesSale()
    {
        // Arrange
        var sale = SaleServiceTestData.GetTestSale();
        var originalTotal = sale.TotalAmount;
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
            
        // Act
        var result = await _saleService.CreateSaleAsync(sale, CancellationToken.None);
        
        // Assert
        Assert.Equal(originalTotal, result.TotalAmount);
        await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(
            Arg.Is<SaleCreatedEvent>(e => e.Sale == sale),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateSaleAsync_VerifiesSaleExistsAndUpdatesSale()
    {
        // Arrange
        var existingSale = SaleServiceTestData.GetTestSale();
        var saleToUpdate = SaleServiceTestData.GetTestSale();
        saleToUpdate.Id = existingSale.Id; // Same ID
        saleToUpdate.SaleNumber = "UPDATED-001"; // Changed number
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
            
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(saleToUpdate);
            
        // Act
        var result = await _saleService.UpdateSaleAsync(saleToUpdate, CancellationToken.None);
        
        // Assert
        Assert.Equal("UPDATED-001", result.SaleNumber);
        await _saleRepository.Received(1).GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(saleToUpdate, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(
            Arg.Is<SaleModifiedEvent>(e => e.Sale == saleToUpdate),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateSaleAsync_ThrowsExceptionIfSaleDoesNotExist()
    {
        // Arrange
        var saleToUpdate = SaleServiceTestData.GetTestSale();
        
        _saleRepository.GetByIdAsync(saleToUpdate.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null!);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _saleService.UpdateSaleAsync(saleToUpdate, CancellationToken.None));
            
        await _saleRepository.Received(1).GetByIdAsync(saleToUpdate.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(0).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelSaleAsync_SetsSaleAsCancelledAndUpdates()
    {
        // Arrange
        var sale = SaleServiceTestData.GetTestSale();
        
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
            
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(x => {
                var updatedSale = (Sale)x[0];
                return Task.FromResult(updatedSale);
            });
            
        // Act
        var result = await _saleService.CancelSaleAsync(sale.Id, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsCancelled);
        await _saleRepository.Received(1).GetByIdAsync(sale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.IsCancelled), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(
            Arg.Is<SaleCancelledEvent>(e => e.Sale.Id == sale.Id),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelSaleAsync_ThrowsExceptionIfSaleDoesNotExist()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        
        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null!);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _saleService.CancelSaleAsync(saleId, CancellationToken.None));
            
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        await _saleRepository.Received(0).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelItemAsync_CancelsItemAndUpdatesTotal()
    {
        // Arrange
        var sale = SaleServiceTestData.GetTestSale();
        var itemToCancel = sale.Items.First();
        var itemId = itemToCancel.Id;
        var originalTotal = sale.TotalAmount;
        var itemTotal = itemToCancel.TotalPrice;
        
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
            
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(x => {
                var updatedSale = (Sale)x[0];
                return Task.FromResult(updatedSale);
            });
            
        // Act
        var result = await _saleService.CancelItemAsync(sale.Id, itemId, CancellationToken.None);
        
        // Assert
        var cancelledItem = result.Items.First(i => i.Id == itemId);
        Assert.True(cancelledItem.IsCancelled);
        Assert.Equal(originalTotal - itemTotal, result.TotalAmount);
        
        await _saleRepository.Received(1).GetByIdAsync(sale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == originalTotal - itemTotal), 
            Arg.Any<CancellationToken>());
            
        await _mediator.Received(1).Publish(
            Arg.Is<ItemCancelledEvent>(e => e.Sale.Id == sale.Id && e.Item.Id == itemId),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelItemAsync_ThrowsExceptionIfItemDoesNotExist()
    {
        // Arrange
        var sale = SaleServiceTestData.GetTestSale();
        var nonExistentItemId = Guid.NewGuid();
        
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _saleService.CancelItemAsync(sale.Id, nonExistentItemId, CancellationToken.None));
            
        await _saleRepository.Received(1).GetByIdAsync(sale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(0).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetSaleByIdAsync_ReturnsSaleIfFound()
    {
        // Arrange
        var sale = SaleServiceTestData.GetTestSale();
        
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
            
        // Act
        var result = await _saleService.GetSaleByIdAsync(sale.Id, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(sale.Id, result.Id);
        await _saleRepository.Received(1).GetByIdAsync(sale.Id, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetSaleByIdAsync_ReturnsNullIfNotFound()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        
        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null);
            
        // Act
        var result = await _saleService.GetSaleByIdAsync(saleId, CancellationToken.None);
        
        // Assert
        Assert.Null(result);
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }
}