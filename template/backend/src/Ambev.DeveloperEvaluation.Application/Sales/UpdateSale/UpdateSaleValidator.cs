using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand.
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of UpdateSaleValidator.
    /// </summary>
    public UpdateSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required.");
            
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
            .SetValidator(new SaleItemDtoValidator());
    }
}

/// <summary>
/// Validator for SaleItemDto.
/// </summary>
public class SaleItemDtoValidator : AbstractValidator<SaleItemDto>
{
    /// <summary>
    /// Initializes a new instance of SaleItemDtoValidator.
    /// </summary>
    public SaleItemDtoValidator()
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