using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Castle.Core.Resource;
using System;
using System.IO;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        using CarDealerContext context = new CarDealerContext();

        //string inputXml = File.ReadAllText(@"../../../Datasets/sales.xml");
        //string result = ImportSales(context, inputXml);

        string result = GetSalesWithAppliedDiscount(context);

        Console.WriteLine(result);

    }
    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
    }

    // 09. Import Suppliers
    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportSupplierDto[] suppliersDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

        ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

        foreach (ImportSupplierDto supplierDto in suppliersDtos)
        {
            if (string.IsNullOrEmpty(supplierDto.Name))
            {
                continue;
            }

            Supplier supplier = mapper.Map<Supplier>(supplierDto);
            validSuppliers.Add(supplier);
        }

        context.Suppliers.AddRange(validSuppliers);
        context.SaveChanges();

        return $"Successfully imported {validSuppliers.Count}";
    }

    // 10. Import Parts
    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportPartDto[] partsDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

        ICollection<Part> validParts = new HashSet<Part>();

        Supplier[] suppliersArr = context.Suppliers.ToArray();
        foreach (ImportPartDto partDto in partsDtos)
        {
            if (!suppliersArr.Any(x => x.Id == partDto.SupplierId))
            {
                continue;
            }

            Part parts = mapper.Map<Part>(partDto);
            validParts.Add(parts);
        }
        context.Parts.AddRange(validParts);
        context.SaveChanges();

        return $"Successfully imported {validParts.Count}";
    }

    // 11. Import Cars
    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportCarDto[] carsDtos = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

        ICollection<Car> validCars = new HashSet<Car>();
        Part[] partsArr = context.Parts.ToArray();

        foreach (ImportCarDto carDto in carsDtos)
        {
            if (string.IsNullOrEmpty(carDto.Make) ||
                string.IsNullOrEmpty(carDto.Model))
            {
                continue;
            }
            Car car = mapper.Map<Car>(carDto);

            foreach (var partDto in carDto.Parts.DistinctBy(p => p.Id))
            {
                if (!partsArr.Any(x => x.Id == partDto.Id))
                {
                    continue;
                }

                PartCar carPart = new PartCar()
                {
                    PartId = partDto.Id
                };
                car.PartsCars.Add(carPart);
            }

            validCars.Add(car);
        }

        context.Cars.AddRange(validCars);
        context.SaveChanges();

        return $"Successfully imported {validCars.Count}";
    }

    // 12. Import Customers
    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportCustomerDto[] customersDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");
        ICollection<Customer> validCustomers = new HashSet<Customer>();

        foreach (ImportCustomerDto customerDto in customersDtos)
        {
            if (string.IsNullOrEmpty(customerDto.Name))
            {
                continue;
            }

            Customer customer = mapper.Map<Customer>(customerDto);
            validCustomers.Add(customer);
        }
        context.Customers.AddRange(validCustomers);
        context.SaveChanges();

        return $"Successfully imported {validCustomers.Count}";
    }

    // 13. Import Sales
    public static string ImportSales(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportSaleDto[] salesDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

        ICollection<Sale> validSales = new HashSet<Sale>();

        Car[] carsArr = context.Cars.ToArray();

        foreach (ImportSaleDto saleDto in salesDtos)
        {
            if (!carsArr.Any(c => c.Id == saleDto.CarId))
            {
                continue;
            }

            Sale sale = mapper.Map<Sale>(saleDto);
            validSales.Add(sale);
        }

        context.Sales.AddRange(validSales);
        context.SaveChanges();

        return $"Successfully imported {validSales.Count}";
    }

    // 14. Export Cars With Distance
    public static string GetCarsWithDistance(CarDealerContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportCarDto[] cars = context.Cars
            .Where(c => c.TraveledDistance > 2000000)
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize<ExportCarDto[]>(cars, "cars");
    }

    // 15. Export Cars From Make BMW
    public static string GetCarsFromMakeBmw(CarDealerContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportCarBmwDto[] bmwCars = context.Cars
            .Where(c => c.Make.ToUpper() == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .ProjectTo<ExportCarBmwDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize<ExportCarBmwDto[]>(bmwCars, "cars");
    }

    // 16. Export Local Suppliers
    public static string GetLocalSuppliers(CarDealerContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportLocalSuplierDto[] localSuplierDtos = context.Suppliers
            .Where(s => !s.IsImporter)
            .Select(s => new ExportLocalSuplierDto
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count()
            })
            .ToArray();

        return xmlHelper.Serialize<ExportLocalSuplierDto[]>(localSuplierDtos, "suppliers");
    }

    // 17. Export Cars With Their List Of Parts

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportCarWithPartsDto[] carsWithParts = context.Cars
            .OrderByDescending(c => c.TraveledDistance)
            .ThenBy(c => c.Model)
            .Take(5)
            .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
            //.Select(c => new ExportCarWithPartsDto
            //{
            //    Make = c.Make,
            //    Model = c.Model,
            //    TraveledDistance = c.TraveledDistance,
            //    Parts = c.PartsCars
            //        .Select(pc => new ExportPartDto
            //        {
            //            Name = pc.Part.Name,
            //            Price = pc.Part.Price
            //        })
            //        .OrderByDescending(p => p.Price)
            //        .ToArray()

            //})
            .ToArray();

        return xmlHelper.Serialize(carsWithParts, "cars");
    }

    // 18. Export Total Sales By Customer

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var tmp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    c.Name,
                    BoughtCars = c.Sales.Count(),
                    TotalSpentMoney = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                            ? s.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2))
                            : s.Car.PartsCars.Sum(p => (double)p.Part.Price)
                    }).ToArray(),
                })
                .ToArray();

        ExportCustomerTotalSalesDto[] customersSales = tmp
            .OrderByDescending(c => c.TotalSpentMoney.Sum(x => x.Prices))
            .Select(c => new ExportCustomerTotalSalesDto
            {
                Name = c.Name,
                BoughtCars = c.BoughtCars,
                SpentMoney = c.TotalSpentMoney.Sum(si => si.Prices).ToString("f2"),

            })
            .ToArray();

        return xmlHelper.Serialize<ExportCustomerTotalSalesDto[]>(customersSales, "customers");
    }

    // 19. Export Sales With Applied Discount

    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportSaleWithDiscountDto[] salesDto = context.Sales
            .Select(s => new ExportSaleWithDiscountDto
            {
                Car = new ExportSaleWithDiscountCarDto
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                Discount = (int)s.Discount,
                Name = s.Customer.Name,
                Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(pc => pc.Part.Price) * ((100 - s.Discount) / 100)), 4)
            })
            .ToArray();

        return xmlHelper.Serialize<ExportSaleWithDiscountDto[]>(salesDto, "sales");
    }


}