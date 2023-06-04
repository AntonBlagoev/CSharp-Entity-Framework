namespace CarDealer.DTOs.Export;

using System.Xml.Serialization;

[XmlType("customer")]
public class ExportCustomerTotalSalesDto
{
    [XmlAttribute("full-name")]
    public string Name { get; set; } = null!;

    [XmlAttribute("bought-cars")]
    public int BoughtCars { get; set; }

    [XmlAttribute("spent-money")]
    public string SpentMoney { get; set; } = null!;

}
