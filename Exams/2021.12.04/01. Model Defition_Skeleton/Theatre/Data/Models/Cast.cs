namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CastFullNameMaxLenght)]
        public string FullName { get; set; } = null!;

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        [Required]
        public virtual Play Play { get; set; } = null!;
    }
}

