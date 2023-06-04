namespace ProductShop.DTOs.Export;

using ProductShop.Models;
using System.Xml.Serialization;



[XmlType("Users")]
public class ExportUsersWithProductsUsersDto
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("users")]
    public ExportUsersWithProductsUserDto[] User { get; set; } = null!;    
}


[XmlType("User")]
public class ExportUsersWithProductsUserDto
{
    [XmlElement("firstName")]
    public string FirstName { get; set; } = null!;

    [XmlElement("lastName")]
    public string LastName { get; set; } = null!;

    [XmlElement("age")]
    public int? Age { get; set; }

    public ExportUsersWithProductsSoldProductsDto? SoldProducts { get; set; }
}

[XmlType("SoldProducts")]
public class ExportUsersWithProductsSoldProductsDto
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("products")]
    public ExportUsersWithProductsProductDto[]? SoldProducts { get; set; }
}

[XmlType("Product")]
public class ExportUsersWithProductsProductDto
{
    [XmlElement("name")]
    public string Name { get; set; } = null!;

    [XmlElement("price")]
    public decimal Price { get; set; }
}
