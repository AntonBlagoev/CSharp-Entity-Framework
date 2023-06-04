namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ExportCoachDto

    {
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlElement("CoachName")]
        public string Name { get; set; } = null!;

        [XmlArray("Footballers")]
        public ExportFootballerDto[]? Footballers { get; set; }
    }
}
