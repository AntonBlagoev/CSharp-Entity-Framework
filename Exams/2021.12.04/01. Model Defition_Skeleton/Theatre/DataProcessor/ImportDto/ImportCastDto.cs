namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Common;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement("FullName")]
        [Required]
        [MinLength(ValidationConstants.CastFullNameMinLenght)]
        [MaxLength(ValidationConstants.CastFullNameMaxLenght)]
        public string FullName { get; set; } = null!;

        [XmlElement("IsMainCharacter")]
        [Required]
        public bool IsMainCharacter { get; set; }

        [XmlElement("PhoneNumber")]
        [Required]
        [RegularExpression(ValidationConstants.CastPhoneNumberRegEx)]
        public string PhoneNumber { get; set; } = null!;

        [XmlElement("PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
