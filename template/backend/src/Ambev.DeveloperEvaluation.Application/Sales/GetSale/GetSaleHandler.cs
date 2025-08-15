using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests.
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult?>
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler.
    /// </summary>
    /// <param name="saleService">The sale service.</param>
    /// <param name="mapper">The mapper.</param>
    public GetSaleHandler(
        ISaleService saleService,
        IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleCommand request.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale details if found, null otherwise.</returns>
    public async Task<GetSaleResult?> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new GetSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        // Get the sale
        var sale = await _saleService.GetSaleByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            return null;
            
        // Map to result
        return _mapper.Map<GetSaleResult>(sale);
    }
}