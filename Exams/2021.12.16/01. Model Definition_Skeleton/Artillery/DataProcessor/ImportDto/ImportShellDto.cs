namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement("ShellWeight")]
        [Required]
        [Range(ValidationConstants.ShellWeightMinValue, ValidationConstants.ShellWeightMaxValue)]
        public double ShellWeight { get; set; }

        [XmlElement("Caliber")]
        [Required]
        [MinLength(ValidationConstants.ShelCaliberMinLenght)]
        [MaxLength(ValidationConstants.ShelCaliberMaxLenght)]
        public string Caliber { get; set; } = null!;
    }
}
