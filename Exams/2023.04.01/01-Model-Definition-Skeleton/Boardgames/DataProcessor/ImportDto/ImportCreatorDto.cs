namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [XmlElement("FirstName")]
        [Required]
        [MinLength(ValidationConstants.CreatorFirstNameMinLenght)]
        [MaxLength(ValidationConstants.CreatorFirstNameMaxLenght)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MinLength(ValidationConstants.CreatorLastNameMinLenght)]
        [MaxLength(ValidationConstants.CreatorLastNameMaxLenght)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardgameDto[]? Boardgames { get; set; }

    }
}
