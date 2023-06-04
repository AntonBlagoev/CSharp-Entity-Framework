namespace VaporStore.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.UserUsernameMaxLenght)]
        public string Username { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public int Age { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
