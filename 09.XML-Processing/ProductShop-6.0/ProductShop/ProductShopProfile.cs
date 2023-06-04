namespace ProductShop;

using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Reflection.Metadata;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        // User
        this.CreateMap<ImportUserDto, User>();
        this.CreateMap<User, ExportSoldProductUserDto>()
            .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold
                                                                        .Select(s => new ExportSoldProductDto
                                                                        {
                                                                            Name = s.Name,
                                                                            Price = s.Price
                                                                        })));
        //this.CreateMap<User, ExportSoldProductDto>();
            


        // Product
        this.CreateMap<ImportProductDto, Product>().ForSourceMember(s => s.BuyerId, opt => opt.DoNotValidate());
        this.CreateMap<Product, ExportProductDto>().ForSourceMember(s => s.BuyerId, opt => opt.DoNotValidate());



        // Category
        this.CreateMap<ImportCategoryDto, Category>();

        // CategoryProduct
        this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

    }

}
