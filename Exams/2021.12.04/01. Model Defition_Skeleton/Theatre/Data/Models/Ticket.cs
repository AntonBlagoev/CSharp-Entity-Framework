namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public sbyte RowNumber { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }
        [Required]
        public virtual Play Play { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Theatre))]
        public int TheatreId { get; set; }
        [Required]
        public virtual Theatre Theatre { get; set; } = null!;
    }
}

