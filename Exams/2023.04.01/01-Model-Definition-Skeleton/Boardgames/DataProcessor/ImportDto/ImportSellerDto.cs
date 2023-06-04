using Boardgames.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstants.SellerNameMinLenght)]
        [MaxLength(ValidationConstants.SellerNameMaxLenght)]
        public string Name { get; set; } = null!;

        [JsonProperty("Address")]
        [Required]
        [MinLength(ValidationConstants.SellerAddressMinLenght)]
        [MaxLength(ValidationConstants.SellerAddressMaxLenght)]
        public string Address { get; set; } = null!;

        [JsonProperty("Country")]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty("Website")]
        [Required]
        [RegularExpression(ValidationConstants.SellerWebsiteRegEx)]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public int[]? Boardgames { get; set; }
    }
}
