using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Validator for CancelSaleCommand.
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of CancelSaleValidator.
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required.");
    }
}