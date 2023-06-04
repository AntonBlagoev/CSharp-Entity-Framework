namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;

    public class ImportTheatreDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstants.TheatreNameMinLenght)]
        [MaxLength(ValidationConstants.TheatreNameMaxLenght)]
        public string Name { get; set; } = null!;

        [JsonProperty("NumberOfHalls")]
        [Required]
        [Range(ValidationConstants.TheatreNumberOfHallsMinValue, ValidationConstants.TheatreNumberOfHallsMaxValue)]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty("Director")]
        [Required]
        [MinLength(ValidationConstants.TheatreDirectorMinLenght)]
        [MaxLength(ValidationConstants.TheatreDirectorMaxLenght)]
        public string Director { get; set; } = null!;

        [JsonProperty("Tickets")]
        public ImportTicketDto[]? Tickets { get; set; }
    }
}
