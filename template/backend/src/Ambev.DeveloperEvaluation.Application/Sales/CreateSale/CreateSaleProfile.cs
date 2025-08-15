using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// AutoMapper profile for create sale operations.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of CreateSaleProfile.
    /// </summary>
    public CreateSaleProfile()
    {
        // Map from result to domain entities
        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<Customer, CreateCustomerResultDto>();
        CreateMap<Branch, CreateBranchResultDto>();
        CreateMap<SaleItem, CreateSaleItemDetailResultDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Product, CreateProductResultDto>();
    }
}