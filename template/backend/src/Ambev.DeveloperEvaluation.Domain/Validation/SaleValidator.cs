using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale entity.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    /// <summary>
    /// Initializes a new instance of SaleValidator.
    /// </summary>
    public SaleValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithErrorCode("Sale.SaleNumberRequired")
            .WithMessage("Sale number is required.");

        RuleFor(x => x.SaleDate)
            .NotEmpty()
            .WithErrorCode("Sale.SaleDateRequired")
            .WithMessage("Sale date is required.");

        RuleFor(x => x.Customer)
            .NotNull()
            .WithErrorCode("Sale.CustomerRequired")
            .WithMessage("Customer is required.");

        RuleFor(x => x.Branch)
            .NotNull()
            .WithErrorCode("Sale.BranchRequired")
            .WithMessage("Branch is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithErrorCode("Sale.ItemsRequired")
            .WithMessage("Sale must have at least one item.");
    }
}