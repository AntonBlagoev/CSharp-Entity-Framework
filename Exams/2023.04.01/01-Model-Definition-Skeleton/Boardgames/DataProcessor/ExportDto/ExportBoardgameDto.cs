using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameDto
    {
        [XmlElement("BoardgameName")]
        public string Name { get; set; } = null!;

        [XmlElement("BoardgameYearPublished")]
        public int YearPublished { get; set; }
    }
}
