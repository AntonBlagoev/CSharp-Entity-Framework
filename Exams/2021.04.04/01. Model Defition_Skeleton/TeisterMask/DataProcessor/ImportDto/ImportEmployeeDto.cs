namespace TeisterMask.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using TeisterMask.Common;

    public class ImportEmployeeDto
    {
        [JsonProperty("Username")]
        [Required]
        [MinLength(ValidationConstants.EmployeeUsernameMinLength)]
        [MaxLength(ValidationConstants.EmployeeUsernameMaxLength)]
        [RegularExpression(ValidationConstants.EmployeeUsernameRegEx)]
        public string Username { get; set; } = null!;

        [JsonProperty("Email")]
        [Required]
        [RegularExpression(ValidationConstants.EmployeeEmailRegEx)]
        public string Email { get; set; } = null!;

        [JsonProperty("Phone")]
        [Required]
        [RegularExpression(ValidationConstants.EmployeePhoneRegEx)]
        public string Phone { get; set; } = null!;

        [JsonProperty("Tasks")]
        public int[]? Tasks { get; set; }
    }
}
