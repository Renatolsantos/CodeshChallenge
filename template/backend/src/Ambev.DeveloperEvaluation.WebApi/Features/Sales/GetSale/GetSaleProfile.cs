using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// AutoMapper profile for get sale API operations.
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of GetSaleProfile.
    /// </summary>
    public GetSaleProfile()
    {
        // Map from application result to response
        CreateMap<GetSaleResult, GetSaleResponse>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<Application.Sales.GetSale.CustomerDto, GetCustomerDto>();
        CreateMap<Application.Sales.GetSale.BranchDto, GetBranchDto>();
        CreateMap<Application.Sales.GetSale.SaleItemDetailDto, GetSaleItemDetailDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Application.Sales.GetSale.ProductDto, GetProductDto>();
    }
}