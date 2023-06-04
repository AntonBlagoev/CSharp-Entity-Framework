namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using Common;

    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string Title { get; set; } = null!;

        [XmlElement("Type")]
        [Required]
        public string Type { get; set; } = null!;

        [XmlElement("Key")]
        [Required]
        [RegularExpression(ValidationConstants.PurchaseProductKeyRegEx)]
        public string ProductKey { get; set; } = null!;

        [XmlElement("Card")]
        [Required]
        [RegularExpression(ValidationConstants.CardNumberRegEx)]
        public string Card { get; set; } = null!;

        [XmlElement("Date")]
        [Required]
        public string Date { get; set; } = null!;


    }
}
