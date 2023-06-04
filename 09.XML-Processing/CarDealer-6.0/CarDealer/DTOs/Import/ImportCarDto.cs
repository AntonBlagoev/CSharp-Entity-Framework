namespace CarDealer.DTOs.Import;

using CarDealer.Models;
using System.Xml.Serialization;

[XmlType("Car")]
public class ImportCarDto
{
    [XmlElement("make")]
    public string Make { get; set; } = null!;

    [XmlElement("model")]
    public string Model { get; set; } = null!;

    [XmlElement("traveledDistance")]
    public long TraveledDistance { get; set; }

    [XmlArray("parts")]
    public ImportCarPartsDto[] Parts { get; set; } = null!;
}
