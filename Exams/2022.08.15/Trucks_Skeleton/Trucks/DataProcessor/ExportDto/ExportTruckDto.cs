namespace Trucks.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;

    [XmlType("Truck")]
    public class ExportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        public string? RegistrationNumber { get; set; }

        [XmlElement("Make")]
        public string Make { get; set; }
    }
}
