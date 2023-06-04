namespace ProductShop;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


using Data;
using DTOs.Import;
using Models;
using Castle.Core.Internal;
using System.Linq;
using ProductShop.DTOs.Export;
using System.Xml.Linq;
using System.Diagnostics;

public class StartUp
{
    public static void Main()
    {
        using var context = new ProductShopContext();

        // string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

        // string result = ImportCategoryProducts(context, inputJson);

        string result = GetUsersWithProducts(context);

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

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();
        ImportUserDto[]? userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

        User[] users = mapper.Map<User[]>(userDtos);

        context.Users.AddRange(users);
        context.SaveChanges();
        return $"Successfully imported {users.Length}";
    }

    // 02. Import Products

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportProductDto[]? productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

        Product[] products = mapper.Map<Product[]>(productDtos);

        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    // 03. Import Categories
    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportCategoryDto[]? categorieDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

        ICollection<Category> validCategories = new HashSet<Category>();
        foreach (ImportCategoryDto categoryDto in categorieDtos)
        {
            if (String.IsNullOrEmpty(categoryDto.Name))
            {
                continue;
            }
            Category category = mapper.Map<Category>(categoryDto);
            validCategories.Add(category);
        }

        context.Categories.AddRange(validCategories);
        context.SaveChanges();

        return $"Successfully imported {validCategories.Count}";
    }

    // 04. Import Categories and Products

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();
        ImportCategoryProductDto[]? categoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

        CategoryProduct[] categoryProduct = mapper.Map<CategoryProduct[]>(categoryProductDtos);


        context.CategoriesProducts.AddRange(categoryProduct);
        context.SaveChanges();

        return $"Successfully imported {categoryProduct.Length}";

    }

    // 05. Export Products In Range

    public static string GetProductsInRange(ProductShopContext context)
    {
        // #Anonymous object + Manual Mapping
        //var product = context.Products
        //    .Where(p => p.Price >= 500 && p.Price <= 1000)
        //    .OrderBy(p => p.Price)
        //    .Select(p => new
        //    {
        //        name = p.Name,
        //        price = p.Price,
        //        seller = p.Seller.FirstName + " " + p.Seller.LastName
        //    })
        //    .AsNoTracking()
        //    .ToArray();

        // #DTO + AutoMapper
        IMapper mapper = CreateMapper();
        ExportProductsInRangeDto[] productDtos = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .AsNoTracking()
            .ProjectTo<ExportProductsInRangeDto>(mapper.ConfigurationProvider)
            .ToArray();

        return JsonConvert.SerializeObject(productDtos, Formatting.Indented);

    }

    // 06. Export Sold Products

    public static string GetSoldProducts(ProductShopContext context)
    {
        var usersWitholdProducts = context.Users
            .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold
                    .Where(b => b.Buyer != null)
                    .Select(sp => new
                    {
                        name = sp.Name,
                        price = sp.Price,
                        buyerFirstName = sp.Buyer.FirstName,
                        buyerLastName = sp.Buyer.LastName
                    })
                    .ToArray()
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(usersWitholdProducts, Formatting.Indented);

    }

    // 07. Export Categories By Products Count
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categoriesProducts = context.Categories
            .OrderByDescending(c => c.CategoriesProducts.Count)
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts.Count,
                averagePrice = c.CategoriesProducts.Average(p => p.Product.Price).ToString("f2"),
                totalRevenue = c.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(categoriesProducts, Formatting.Indented);
    }

    // 08. Export Users and Products

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var usersWitholdProducts = context.Users
            .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold.Count(p => p.Buyer != null),
                    products = u.ProductsSold
                    .Where(b => b.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    })
                    .ToArray()
                }
            })
            .OrderByDescending(u => u.soldProducts.count)
            .AsNoTracking()
            .ToArray();

        var userInfo = new
        {
            usersCount = usersWitholdProducts.Count(),
            users = usersWitholdProducts

        };

        return JsonConvert.SerializeObject(userInfo, Formatting.Indented, 
            new JsonSerializerSettings() 
            { 
                NullValueHandling = NullValueHandling.Ignore}
            );
    }




}