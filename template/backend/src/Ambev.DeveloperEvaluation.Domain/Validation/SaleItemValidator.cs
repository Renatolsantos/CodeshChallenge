using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the SaleItem entity.
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    /// <summary>
    /// Initializes a new instance of SaleItemValidator.
    /// </summary>
    public SaleItemValidator()
    {
        RuleFor(x => x.Product)
            .NotNull()
            .WithErrorCode("SaleItem.ProductRequired")
            .WithMessage("Product is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithErrorCode("SaleItem.QuantityMustBePositive")
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Quantity)
            .LessThanOrEqualTo(20)
            .WithErrorCode("SaleItem.QuantityExceedsLimit")
            .WithMessage("Quantity cannot exceed 20 items.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithErrorCode("SaleItem.UnitPriceMustBePositive")
            .WithMessage("Unit price must be greater than zero.");

        // Only items with 4 or more units can have discounts
        RuleFor(x => x.DiscountPercentage)
            .Equal(0)
            .When(x => x.Quantity < 4)
            .WithErrorCode("SaleItem.NoDiscountForLessThanFourItems")
            .WithMessage("Purchases below 4 items cannot have a discount.");
    }
}