namespace Footballers.Data.Models
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;

    public class Coach
    {
        public Coach()
        {
            this.Footballers = new HashSet<Footballer>();
        }
        [Key]
        public  int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstraints.CoachNameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        public string Nationality { get; set; } = null!;

        public virtual ICollection<Footballer> Footballers { get; set; }
    }
}
