using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// AutoMapper profile for update sale operations.
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of UpdateSaleProfile.
    /// </summary>
    public UpdateSaleProfile()
    {
        // Map from domain entities to result
        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<Customer, UpdateCustomerDto>();
        CreateMap<Branch, UpdateBranchDto>();
        CreateMap<SaleItem, UpdateSaleItemDetailDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Product, UpdateProductDto>();
    }
}