using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler.
    /// </summary>
    /// <param name="saleService">The sale service.</param>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="branchRepository">The branch repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="mapper">The mapper.</param>
    public UpdateSaleHandler(
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
    /// Handles the UpdateSaleCommand request.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale details.</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        // Get the existing sale
        var existingSale = await _saleService.GetSaleByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found.");
            
        // Get the customer
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found.");
            
        // Get the branch
        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {command.BranchId} not found.");
            
        // Update the sale properties
        existingSale.SaleNumber = command.SaleNumber;
        existingSale.SaleDate = command.SaleDate;
        existingSale.Customer = customer;
        existingSale.Branch = branch;
        
        // Clear existing items and add the new ones
        existingSale.Items.Clear();
        
        // Create and add the items
        foreach (var itemDto in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");
                
            // Create a new sale item or reuse existing one
            SaleItem saleItem;
            
            if (itemDto.Id.HasValue)
            {
                // Find the existing item
                saleItem = existingSale.Items.FirstOrDefault(i => i.Id == itemDto.Id.Value) ?? new SaleItem(product, itemDto.Quantity);
                
                // Update properties
                saleItem.Product = product;
                saleItem.UpdateQuantity(itemDto.Quantity);
            }
            else
            {
                saleItem = new SaleItem(product, itemDto.Quantity);
            }
            
            existingSale.AddItem(saleItem);
        }
        
        // Save the sale
        var updatedSale = await _saleService.UpdateSaleAsync(existingSale, cancellationToken);
        
        // Map to result
        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}