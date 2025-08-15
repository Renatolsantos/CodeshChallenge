using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler.
    /// </summary>
    /// <param name="saleService">The sale service.</param>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="branchRepository">The branch repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="mapper">The mapper.</param>
    public CreateSaleHandler(
        ISaleService saleService,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleService = saleService;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale details.</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found.");
            
        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {command.BranchId} not found.");
            
        var sale = new Sale
        {
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Customer = customer,
            Branch = branch
        };
        
        foreach (var itemDto in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");
                
            var saleItem = new SaleItem(product, itemDto.Quantity);
            sale.AddItem(saleItem);
        }
        
        var createdSale = await _saleService.CreateSaleAsync(sale, cancellationToken);
        
        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}