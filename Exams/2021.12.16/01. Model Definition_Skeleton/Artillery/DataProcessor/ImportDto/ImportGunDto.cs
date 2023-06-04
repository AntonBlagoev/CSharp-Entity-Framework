namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Common;
    using Newtonsoft.Json;


    public class ImportGunDto
    {
        [JsonProperty("ManufacturerId")]
        [Required]
        public int ManufacturerId { get; set; }

        [JsonProperty("GunWeight")]
        [Required]
        [Range(ValidationConstants.GunWeightMinValue, ValidationConstants.GunWeightMaxValue)]
        public int GunWeight { get; set; }

        [JsonProperty("BarrelLength")]
        [Required]
        [Range(ValidationConstants.GunBarrelLengthMinValue, ValidationConstants.GunBarrelLengthMaxValue)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [JsonProperty("Range")]
        [Required]
        [Range(ValidationConstants.GunRangeMinValue, ValidationConstants.GunRangeMaxValue)]
        public int Range { get; set; }

        [JsonProperty("GunType")]
        [Required]
        public string GunType { get; set; } = null!;

        [JsonProperty("ShellId")]
        [Required]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportCountryIdDto[]? Countries { get; set; }
    }
}
