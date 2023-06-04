
namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Common;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.DespatcherNameMinLenght)]
        [MaxLength(ValidationConstants.DespatcherNameMaxLenght)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        [Required]
        public string? Position { get; set; }

        [XmlArray("Trucks")]
        public ImportTruckDto[]? Trucks { get; set; }
    }
}
