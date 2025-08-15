using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// AutoMapper profile for get sale operations.
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of GetSaleProfile.
    /// </summary>
    public GetSaleProfile()
    {
        // Map from domain entities to result
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<Customer, CustomerDto>();
        CreateMap<Branch, BranchDto>();
        CreateMap<SaleItem, SaleItemDetailDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Product, ProductDto>();
    }
}