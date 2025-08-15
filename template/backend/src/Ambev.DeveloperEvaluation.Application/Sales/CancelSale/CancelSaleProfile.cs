using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// AutoMapper profile for cancel sale operations.
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of CancelSaleProfile.
    /// </summary>
    public CancelSaleProfile()
    {
        // Map from domain entities to result
        CreateMap<Sale, CancelSaleResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
}