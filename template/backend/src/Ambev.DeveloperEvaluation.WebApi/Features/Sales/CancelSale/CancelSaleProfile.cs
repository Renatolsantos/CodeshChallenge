using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// AutoMapper profile for cancel sale API operations.
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of CancelSaleProfile.
    /// </summary>
    public CancelSaleProfile()
    {
        // Map from application result to response
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}