

namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Trucks.Common;
    public class ImportClientDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstants.ClientNameMinLenght)]
        [MaxLength(ValidationConstants.ClientNameMaxLenght)]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [Required]
        [MinLength(ValidationConstants.ClientNationalityMinLenght)]
        [MaxLength(ValidationConstants.ClientNationalityMaxLenght)]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; } = null!;

        [JsonProperty("Trucks")]
        public int[]? TruckId { get; set; }

    }
}
