namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Common;
    using Theatre.Data.Models.Enums;

    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement("Title")]
        [Required]
        [MinLength(ValidationConstants.PlayTitleMinLenght)]
        [MaxLength(ValidationConstants.PlayTitleMaxLenght)]
        public string Title { get; set; } = null!;

        [XmlElement("Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlElement("Raiting")]
        [Required]
        [Range(ValidationConstants.PlayRatingMinValue, ValidationConstants.PlayRatingMaxValue)]
        public float Rating { get; set; }

        [XmlElement("Genre")]
        [Required]
        public string Genre { get; set; }

        [XmlElement("Description")]
        [Required]
        [MaxLength(ValidationConstants.PlayDescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        [XmlElement("Screenwriter")]
        [Required]
        [MinLength(ValidationConstants.PlayScreenwriterMinLenght)]
        [MaxLength(ValidationConstants.PlayScreenwriterMaxLenght)]
        public string Screenwriter { get; set; } = null!;
    }
}
