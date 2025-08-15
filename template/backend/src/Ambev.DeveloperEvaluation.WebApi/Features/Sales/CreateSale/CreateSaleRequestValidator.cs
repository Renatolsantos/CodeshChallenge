using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of CreateSaleRequestValidator.
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required.")
            .MaximumLength(50)
            .WithMessage("Sale number cannot exceed 50 characters.");
            
        RuleFor(x => x.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required.");
            
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required.");
            
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required.");
            
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item.");
            
        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for UpdateSaleItemRequest.
/// </summary>
public class SaleItemRequestValidator : AbstractValidator<SaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of SaleItemRequestValidator.
    /// </summary>
    public SaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");
            
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity cannot exceed 20 items.");
    }
}