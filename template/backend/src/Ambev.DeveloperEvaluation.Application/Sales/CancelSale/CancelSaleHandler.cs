using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests.
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler.
    /// </summary>
    /// <param name="saleService">The sale service.</param>
    /// <param name="mapper">The mapper.</param>
    public CancelSaleHandler(
        ISaleService saleService,
        IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CancelSaleCommand request.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cancelled sale details.</returns>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        var cancelledSale = await _saleService.CancelSaleAsync(command.Id, cancellationToken);
        
        return _mapper.Map<CancelSaleResult>(cancelledSale);
    }
}