namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Common;

    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        [MinLength(ValidationConstants.TruckRegistrationNumberLenght)]
        [MaxLength(ValidationConstants.TruckRegistrationNumberLenght)]
        [RegularExpression(ValidationConstants.TruckRegistrationNumberRegEx)]
        public string? RegistrationNumber { get; set; }

        [XmlElement("VinNumber")]
        [Required]
        [MinLength(ValidationConstants.TruckVinNumberLenght)]
        [MaxLength(ValidationConstants.TruckVinNumberLenght)]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Range(ValidationConstants.TruckTankCapacityMinValue, ValidationConstants.TruckTankCapacityMaxValue)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(ValidationConstants.TruckCargoCapacityMinValue, ValidationConstants.TruckCargoCapacityMaxValue)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        public int MakeType { get; set; }
    }
}