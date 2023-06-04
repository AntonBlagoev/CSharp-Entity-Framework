namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Enums;

    public class Play
    {
        public Play()
        {
            this.Casts = new HashSet<Cast>();
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlayTitleMaxLenght)]
        public string Title { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlayDescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PlayScreenwriterMaxLenght)]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }


    }
}



