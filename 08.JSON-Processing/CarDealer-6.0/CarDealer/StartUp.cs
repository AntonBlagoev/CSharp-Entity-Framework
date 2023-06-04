namespace CarDealer;

using AutoMapper;
using Castle.Core.Resource;
using Data;
using DTOs.Import;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

public class StartUp
{
    public static void Main()
    {
        using var context = new CarDealerContext();

        //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");

        //string result = ImportSales(context, inputJson);

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

    public static string ImportSuppliers(CarDealerContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();
        ImportSuppliersDto[]? supplierDtos = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);

        Supplier[] suppliers = mapper.Map<Supplier[]>(supplierDtos);

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();
        return $"Successfully imported {suppliers.Length}.";
    }

    // 10. Import Parts
    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ICollection<Part> validParts = new HashSet<Part>();

        ImportPartsDto[]? partsDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);

        foreach (ImportPartsDto partDto in partsDtos)
        {
            if (!context.Suppliers.Any(s => s.Id == partDto.SupplierId))
            {
                continue;
            }
            Part part = mapper.Map<Part>(partDto);
            validParts.Add(part);
        }

        context.Parts.AddRange(validParts);
        context.SaveChanges();

        return $"Successfully imported {validParts.Count}.";
    }

    // 11. Import Cars
    public static string ImportCars(CarDealerContext context, string inputJson)
    {
        ImportCarsDto[]? carsDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);

        HashSet<Car> cars = new HashSet<Car>();
        HashSet<PartCar> partsCars = new HashSet<PartCar>();

        foreach (var carDto in carsDtos)
        {
            Car car = new Car()
            {
                Make = carDto.Make,
                Model = carDto.Model,
                TraveledDistance = carDto.TraveledDistance,
            };
            cars.Add(car);

            foreach (var part in carDto.PartsId.Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    Car = car,
                    PartId = part
                };
                partsCars.Add(partCar);
            }
        }
        context.Cars.AddRange(cars);
        context.PartsCars.AddRange(partsCars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count()}.";
    }

    // 12. Import Customers
    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();
        ImportCustomersDto[]? customersDtos = JsonConvert.DeserializeObject<ImportCustomersDto[]>(inputJson);
        Customer[] customers = mapper.Map<Customer[]>(customersDtos);

        //// # Manual Mapping
        //HashSet<Customer> customers = new HashSet<Customer>();
        //foreach (var custDto in customersDtos)
        //{
        //    Customer customer = new Customer()
        //    {
        //        Name = custDto.Name,
        //        BirthDate = custDto.BirthDate,
        //        IsYoungDriver = custDto.IsYoungDriver
        //    };
        //    customers.Add(customer);
        //};

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count()}.";
    }

    // 13. Import Sales
    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        ImportSalesDto[]? salesDtos = JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);

        IMapper mapper = CreateMapper();
        Sale[] sales = mapper.Map<Sale[]>(salesDtos);

        //HashSet<Sale> sales = new HashSet<Sale>();
        //foreach (var saleDto in salesDtos)
        //{
        //    Sale sale = new Sale()
        //    {
        //        CarId = saleDto.CarId,
        //        CustomerId = saleDto.CustomerId,
        //        Discount = saleDto.Discount
        //    };
        //    sales.Add(sale);
        //}

        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {sales.Length}.";
    }

    // 14. Export Ordered Customers

    public static string GetOrderedCustomers(CarDealerContext context)
    {
        var customers = context.Customers
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .Select(c => new
            {
                c.Name,
                BirthDate = c.BirthDate.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
                c.IsYoungDriver
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(customers, Formatting.Indented);
    }

    // 15. Export Cars From Make Toyota
    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        var cars = context.Cars
            .Where(c => c.Make == "Toyota")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Select(c => new
            {
                c.Id,
                c.Make,
                c.Model,
                c.TraveledDistance
            })
            .AsNoTracking()
            .ToArray();


        return JsonConvert.SerializeObject(cars, Formatting.Indented);
    }

    // 16. Export Local Suppliers

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        var suppliers = context.Suppliers
            .Where(s => !s.IsImporter)
            .Select(s => new
            {
                s.Id,
                s.Name,
                PartsCount = s.Parts.Count()
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
    }

    // 17. Export Cars With Their List Of Parts
    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        var carsWithParts = context.Cars
            .Select(c => new
            {
                car = new
                {
                    c.Make,
                    c.Model,
                    c.TraveledDistance,
                },
                parts = c.PartsCars
                .Select(cp => new
                {
                    cp.Part.Name,
                    Price = cp.Part.Price.ToString("f2")
                })
            })
            .ToArray();

        return JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
    }

    // 18. Export Total Sales By Customer

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        var customersTotalSales = context.Customers
            .Where(c => c.Sales.Count() > 0)
            .Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count(),
                spentMoney = c.Sales.SelectMany(x => x.Car.PartsCars.Select(p => p.Part.Price)).Sum()
            })
            .OrderByDescending(c => c.spentMoney)
            .ThenByDescending(c => c.boughtCars)
            .ToArray();

        return JsonConvert.SerializeObject(customersTotalSales, Formatting.Indented);
    }

    // 19. Export Sales With Applied Discount
    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        var salesWithDiscount = context.Sales
            .Take(10)
            .Select(s => new
            {
                car = new
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                customerName = s.Customer.Name,
                discount = $"{s.Discount:f2}",
                price = $"{s.Car.PartsCars.Sum(p => p.Part.Price):f2}",
                priceWithDiscount = $"{(s.Car.PartsCars.Sum(p => p.Part.Price) * (100 - s.Discount)) / 100:f2}",
            })
            .ToArray();

        return JsonConvert.SerializeObject(salesWithDiscount, Formatting.Indented);
    }
}