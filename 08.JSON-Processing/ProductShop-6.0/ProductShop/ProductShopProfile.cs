namespace ProductShop;

using AutoMapper;
using DTOs.Import;
using Models;
using ProductShop.DTOs.Export;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        this.CreateMap<ImportUserDto, User>();

        this.CreateMap<ImportProductDto, Product>();

        // #Anonymous object + Manual Mapping
        //this.CreateMap<Product, ExportProductsInRangeDto>();

        // #DTO + AutoMapper
        this.CreateMap<Product, ExportProductsInRangeDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.ProductPrice, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.ProductSeller, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));


        this.CreateMap<ImportCategoryDto, Category>();

        this.CreateMap<ImportCategoryProductDto, CategoryProduct>();







    }
}
