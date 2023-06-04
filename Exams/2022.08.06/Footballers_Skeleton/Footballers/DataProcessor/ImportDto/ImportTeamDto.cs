namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportTeamDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstraints.TeamNameMinLenght)]
        [MaxLength(ValidationConstraints.TeamNameMaxLenght)]
        [RegularExpression(ValidationConstraints.TeamNameRegEx)]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [Required]
        [MinLength(ValidationConstraints.TeamNationalityMinLenght)]
        [MaxLength(ValidationConstraints.TeamNationalityMaxLenght)]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Trophies")]
        [Required]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public  int[]? FootballersIds { get; set; }
    }
}
