using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// AutoMapper profile for update sale API operations.
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of UpdateSaleProfile.
    /// </summary>
    public UpdateSaleProfile()
    {
        // Map from request to command
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<UpdateSaleItemRequest, SaleItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            
        // Map from application result to response
        CreateMap<UpdateSaleResult, UpdateSaleResponse>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<UpdateCustomerDto, UpdateCustomerResponseDto>();
        CreateMap<UpdateBranchDto, BranchDto>();
        CreateMap<UpdateSaleItemDetailDto, SaleItemDetailDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<UpdateProductDto, ProductDto>();
    }
}