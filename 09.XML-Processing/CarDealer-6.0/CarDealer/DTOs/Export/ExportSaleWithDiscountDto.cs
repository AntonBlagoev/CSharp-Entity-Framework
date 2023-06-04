namespace CarDealer.DTOs.Export;

using CarDealer.Models;
using System.Xml.Serialization;

[XmlType("sale")]
public class ExportSaleWithDiscountDto
{
    [XmlElement("car")]
    public ExportSaleWithDiscountCarDto Car { get; set; }

    [XmlElement("discount")]
    public int Discount { get; set; }

    [XmlElement("customer-name")]
    public string Name { get; set; } = null!;

    [XmlElement("price")]
    public decimal Price { get; set; }

    [XmlElement("price-with-discount")]
    public double PriceWithDiscount { get; set; }


}



