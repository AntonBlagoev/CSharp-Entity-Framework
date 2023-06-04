namespace Boardgames.Data.Models
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;

    public class Seller
    {
        public Seller()
        {
            this.BoardgamesSellers = new HashSet<BoardgameSeller>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.SellerNameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.SellerAddressMaxLenght)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Website { get; set; } = null!;
        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}

