namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstraints.CoachNameMinLenght)]
        [MaxLength(ValidationConstraints.CoachNameMaxLenght)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        public ImportFootballerDto[] Footballers { get; set; } = null!;



    }
}
