using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;
using Newtonsoft.Json;
using VaporStore.Common;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportGameDto
    {
        [JsonProperty("Name")]
        [Required]
        public string Name { get; set; } = null!;

        [JsonProperty("Price")]
        [Required]
        [Range(ValidationConstants.GamePriceMinValue, ValidationConstants.GamePriceMaxValue)]
        public decimal Price { get; set; }

        [JsonProperty("ReleaseDate")]
        [Required]
        public string ReleaseDate { get; set; } = null!;

        [JsonProperty("Developer")]
        [Required]
        public string Developer { get; set; } = null!;

        [JsonProperty("Genre")]
        [Required]
        public string Genre { get; set; } = null!;

        [JsonProperty("Tags")]
        [Required]
        public string[] Tags { get; set; } = null!;
    }
}
