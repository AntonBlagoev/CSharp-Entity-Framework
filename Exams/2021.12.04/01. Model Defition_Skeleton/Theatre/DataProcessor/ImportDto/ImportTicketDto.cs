
namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;

    public class ImportTicketDto
    {
        [JsonProperty("Price")]
        [Required]
        [Range(ValidationConstants.TicketPriceMinValue, ValidationConstants.TicketPriceMaxValue)]
        public decimal Price { get; set; }

        [JsonProperty("RowNumber")]
        [Required]
        [Range(ValidationConstants.TicketRowNumberMinValue, ValidationConstants.TicketRowNumberMaxValue)]
        public sbyte RowNumber { get; set; }

        [JsonProperty("PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
