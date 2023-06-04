namespace CarDealer;

using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        // Supplier
        this.CreateMap<ImportSupplierDto, Supplier>();


        // Part
        this.CreateMap<ImportPartDto, Part>();
        this.CreateMap<Part, ExportPartDto>();


        // Car
        this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
        this.CreateMap<Car, ExportCarDto>();
        this.CreateMap<Car, ExportCarBmwDto>();
        this.CreateMap<Car, ExportSaleWithDiscountCarDto>();

        // KrIsKa7a solution for exercies 17
        this.CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.PartsCars.Select(pc => pc.Part).OrderByDescending(p => p.Price).ToArray()));


        // Customer
        this.CreateMap<ImportCustomerDto, Customer>()
            .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate!, CultureInfo.InvariantCulture)));

        this.CreateMap<Customer, ExportCustomerTotalSalesDto>()
            .ForMember(d => d.BoughtCars, opt => opt.MapFrom(s => s.Sales.Count()))
            .ForMember(d => d.SpentMoney, opt => opt.MapFrom(src => src.Sales.Select(s => s.Car).SelectMany(c => c.PartsCars).Sum(c => c.Part.Price)));

        // Sale
        this.CreateMap<ImportSaleDto, Sale>();
        this.CreateMap<Sale, ExportSaleWithDiscountDto>();
    }
}
