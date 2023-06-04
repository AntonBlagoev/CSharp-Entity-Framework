using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [XmlElement("CountryName")]
        [Required]
        [MinLength(ValidationConstants.CountryNameMinLenght)]
        [MaxLength(ValidationConstants.CountryNameMaxLenght)]
        public string CountryName { get; set; } = null!;

        [XmlElement("ArmySize")]
        [Required]
        [Range(ValidationConstants.CountryArmySizeMinValue, ValidationConstants.CountryArmySizeMaxValue)]
        public int ArmySize { get; set; }
    }
}
