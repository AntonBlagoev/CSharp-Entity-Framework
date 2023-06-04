namespace Boardgames.Data.Models
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;

    public class Creator
    {
        public Creator()
        {
            this.Boardgames = new HashSet<Boardgame>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CreatorFirstNameMaxLenght)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.CreatorLastNameMaxLenght)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Boardgame> Boardgames { get; set; }
    }
}
