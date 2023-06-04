namespace ProductShop;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Core.Internal;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        using ProductShopContext context = new ProductShopContext();

        string inputXml = File.ReadAllText(@"../../../Datasets/products.xml");
        string result = ImportProducts(context, inputXml);

        //string result = GetUsersWithProducts(context);

        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }

    // 01. Import Users

    public static string ImportUsers(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportUserDto[] usersDtos = xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

        ICollection<User> validUsers = new HashSet<User>();

        foreach (ImportUserDto userDto in usersDtos)
        {
            User user = mapper.Map<User>(userDto);
            validUsers.Add(user);

        }
        context.AddRange(validUsers);
        context.SaveChanges();

        return $"Successfully imported {validUsers.Count}";
    }

    // 02. Import Products
    public static string ImportProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ICollection<Product> products = new HashSet<Product>();


        // Product[] products = mapper.Map<Product[]>(productsDtos);

        ImportProductDto[] productsDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");
        foreach (ImportProductDto productDto in productsDtos)
        {
            if (!context.Users.Any(u => u.Id == productDto.SellerId))
            {
                continue;
            }
            Product product = mapper.Map<Product>(productDto);
            products.Add(product);
        }

        //// Manual mapping
        //Product[] products1 = productsDtos
        //    .Select(p => new Product()
        //    {
        //        Name = p.Name,
        //        Price = p.Price,
        //        BuyerId = p.BuyerId == 0 ? null : p.BuyerId,
        //        SellerId = p.SellerId
        //    })
        //    .ToArray();

        context.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Count}";
    }

    // 03. Import Categories
    public static string ImportCategories(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper XmlHelper = new XmlHelper();
        ImportCategoryDto[] categoriesDtos = XmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

        Category[] categories = mapper.Map<Category[]>(categoriesDtos);

        context.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Length}";
    }

    // 04. Import Categories and Products
    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper XmlHelper = new XmlHelper();
        ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

        Category[] categorysArr = context.Categories.ToArray();
        Product[] productsArr = context.Products.ToArray();

        ImportCategoryProductDto[] dtos = XmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");
        foreach (ImportCategoryProductDto dto in dtos)
        {
            if (!categorysArr.Any(c => c.Id == dto.CategoryId) ||
                !productsArr.Any(p => p.Id == dto.ProductId))
            {
                continue;
            }

            var categoryProduct = mapper.Map<CategoryProduct>(dto);
            categoryProducts.Add(categoryProduct);
        }
        context.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count}";
    }


    // 05. Export Products In Range
    public static string GetProductsInRange(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportProductDto[] productsInRange = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .Select(p => new ExportProductDto
            {
                Name = p.Name,
                Price = p.Price,
                Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
            })
            //.ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(productsInRange, "Products");
    }

    // 06. Export Sold Products
    public static string GetSoldProducts(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportSoldProductUserDto[] soldProducts = context.Users
            .Where(u => u.ProductsSold.Any())
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.LastName)
            .Take(5)
            //.Select(u => new ExportSoldProductUserDto
            //{
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    SoldProducts = u.ProductsSold
            //        .Select(x => new ExportSoldProductDto
            //        {
            //            Name = x.Name,
            //            Price = x.Price
            //        })
            //        .ToArray()
            //})
            .ProjectTo<ExportSoldProductUserDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(soldProducts, "Users");
    }

    // 07. Export Categories By Products Count
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportCategoryDto[] categories = context.Categories
            .Select(c => new ExportCategoryDto
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count(),
                AveragePrice = c.CategoryProducts.Select(p => p.Product.Price).Average(),
                TotalRevenue = c.CategoryProducts.Select(p => p.Product.Price).Sum()
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.TotalRevenue)
            .ToArray();

        return xmlHelper.Serialize(categories, "Categories");
    }

    // 08. Export Users and Products

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();
        XmlHelper XmlHelper = new XmlHelper();

        var usersInfo = context.Users
            .Where(u => u.ProductsSold.Any())
            .OrderByDescending(u => u.ProductsSold.Count())
            .Take(10)
            .Select(u => new ExportUsersWithProductsUserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                SoldProducts = new ExportUsersWithProductsSoldProductsDto
                {
                    Count = u.ProductsSold.Count(),
                    SoldProducts =  u.ProductsSold
                    .Select(p => new ExportUsersWithProductsProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .OrderByDescending(u => u.Price)
                    .ToArray()
                }
            })
            .ToArray();

        ExportUsersWithProductsUsersDto usersTotal = new ExportUsersWithProductsUsersDto()
        {
            Count = context.Users.Count(u => u.ProductsSold.Any()),
            User = usersInfo
        };

        return XmlHelper.Serialize(usersTotal, "Users");
    }
}