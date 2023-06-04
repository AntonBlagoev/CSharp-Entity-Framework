namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.BoardgameNameMinLenght)]
        [MaxLength(ValidationConstants.BoardgameNameMaxLenght)]
        public string Name { get; set; } = null!;

        [XmlElement("Rating")]
        [Required]
        [Range(ValidationConstants.BoardgameRatingMinValue, ValidationConstants.BoardgameRatingMaxValue)]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Required]
        [Range(ValidationConstants.BoardgameYearPublishedMinValue, ValidationConstants.BoardgameYearPublishedMaxValue)]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        public string CategoryType { get; set; } = null!;

        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;

    }
}
