using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardDto
    {
        [JsonProperty("Number")]
        [Required]
        [RegularExpression(ValidationConstants.CardNumberRegEx)]
        public string Number { get; set; } = null!;

        [JsonProperty("CVC")]
        [Required]
        [RegularExpression(ValidationConstants.CardCvcRegEx)]
        public string Cvc { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; } = null!;
    }
}
