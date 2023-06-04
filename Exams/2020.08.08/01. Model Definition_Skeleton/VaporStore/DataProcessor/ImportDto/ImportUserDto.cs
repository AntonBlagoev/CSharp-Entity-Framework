using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        [JsonProperty("FullName")]
        [Required]
        [RegularExpression(ValidationConstants.UserFullNameRegEx)]
        public string FullName { get; set; } = null!;

        [JsonProperty("Username")]
        [Required]
        [MinLength(ValidationConstants.UserUsernameMinLenght)]
        [MaxLength(ValidationConstants.UserUsernameMaxLenght)]
        public string Username { get; set; } = null!;

        [JsonProperty("Email")]
        [Required]
        [RegularExpression(ValidationConstants.UserEmailRegEx)]
        public string Email { get; set; } = null!;

        [JsonProperty("Age")]
        [Required]
        [Range(ValidationConstants.UserAgeMinValue, ValidationConstants.UserAgeMaxValue)]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        public ImportCardDto[] Cards { get; set; } = null!;
    }
}
