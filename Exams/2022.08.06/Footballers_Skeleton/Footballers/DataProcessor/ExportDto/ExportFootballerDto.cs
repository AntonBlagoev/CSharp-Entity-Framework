namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;


    [XmlType("Footballer")]
    public class ExportFootballerDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string? PositionType { get; set; }
    }
}
