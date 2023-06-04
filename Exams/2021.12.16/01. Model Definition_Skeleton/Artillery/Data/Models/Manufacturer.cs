namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Common;

    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ManufacturerNameMaxLenght)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.ManufacturerFoundedMaxLenght)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
    }
}

