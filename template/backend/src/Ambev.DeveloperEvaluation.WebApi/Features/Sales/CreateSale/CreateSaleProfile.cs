using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// AutoMapper profile for create sale API operations.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of CreateSaleProfile.
    /// </summary>
    public CreateSaleProfile()
    {
        // Map from request to command
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<SaleItemRequest, SaleItemDto>();
            
        // Map from application result to response
        CreateMap<CreateSaleResult, CreateSaleResponse>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<CreateCustomerResultDto, CreateCustomerResponseDto>();
        CreateMap<CreateBranchResultDto, CreateBranchResponseDto>();
        CreateMap<CreateSaleItemDetailResultDto, CreateSaleItemDetailResponseDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<CreateProductResultDto, CreateProductResponseDto>();
    }
}