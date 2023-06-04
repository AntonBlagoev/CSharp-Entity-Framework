namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using VaporStore.Data.Models.Enums;

    public class Card
    {
        public Card()
        {
            this.Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; } = null!;

        [Required]
        public string Cvc { get; set; } = null!;

        [Required]
        public CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Purchase> Purchases { get; set; }


    }
}

