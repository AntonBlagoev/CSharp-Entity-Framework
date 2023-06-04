namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [XmlElement("ManufacturerName")]
        [Required]
        [MinLength(ValidationConstants.ManufacturerNameMinLenght)]
        [MaxLength(ValidationConstants.ManufacturerNameMaxLenght)]
        public string ManufacturerName { get; set; } = null!;

        [XmlElement("Founded")]
        [Required]
        [MinLength(ValidationConstants.ManufacturerFoundedMinLenght)]
        [MaxLength(ValidationConstants.ManufacturerFoundedMaxLenght)]
        public string Founded { get; set; } = null!;
    }
}
