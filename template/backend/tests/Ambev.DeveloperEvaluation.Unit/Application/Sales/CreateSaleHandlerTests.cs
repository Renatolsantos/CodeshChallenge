using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleService _saleService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;
    
    public CreateSaleHandlerTests()
    {
        _saleService = Substitute.For<ISaleService>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        
        _handler = new CreateSaleHandler(
            _saleService, 
            _customerRepository,
            _branchRepository,
            _productRepository,
            _mapper);
    }
    
    [Fact]
    public async Task Handle_ValidCommand_CreatesSaleAndReturnsResult()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE-001",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<SaleItemDto>
            {
                new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 5 },
                new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 3 }
            }
        };
        
        var customer = SaleTestData.GetTestCustomer();
        customer.Id = command.CustomerId;
        
        var branch = SaleTestData.GetTestBranch();
        branch.Id = command.BranchId;
        
        var product1 = SaleItemTestData.GetTestProduct(100m);
        product1.Id = command.Items[0].ProductId;
        
        var product2 = SaleItemTestData.GetTestProduct(200m);
        product2.Id = command.Items[1].ProductId;
        
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);
            
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);
            
        _productRepository.GetByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>())
            .Returns(product1);
            
        _productRepository.GetByIdAsync(command.Items[1].ProductId, Arg.Any<CancellationToken>())
            .Returns(product2);
        
        var createdSale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Customer = customer,
            Branch = branch,
            TotalAmount = 1050m
        };
        
        _saleService.CreateSaleAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);
            
        var expectedResult = new CreateSaleResult
        {
            Id = createdSale.Id,
            SaleNumber = createdSale.SaleNumber,
            TotalAmount = createdSale.TotalAmount
        };
        
        _mapper.Map<CreateSaleResult>(createdSale)
            .Returns(expectedResult);
            
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedResult, result);
        await _customerRepository.Received(1).GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(command.Items[1].ProductId, Arg.Any<CancellationToken>());
        await _saleService.Received(1).CreateSaleAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_CustomerNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "test124",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<SaleItemDto> { new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns((Customer)null!);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None));
            
        await _customerRepository.Received(1).GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>());
        await _branchRepository.Received(0).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_BranchNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "test124",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<SaleItemDto> { new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        
        var customer = SaleTestData.GetTestCustomer();
        
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);
            
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns((Branch)null!);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None));
            
        await _customerRepository.Received(1).GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>());
        await _productRepository.Received(0).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "test124",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<SaleItemDto> { new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        
        var customer = SaleTestData.GetTestCustomer();
        var branch = SaleTestData.GetTestBranch();
        
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);
            
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);
            
        _productRepository.GetByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>())
            .Returns((Product)null!);
            
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None));
            
        await _customerRepository.Received(1).GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>());
        await _saleService.Received(0).CreateSaleAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}