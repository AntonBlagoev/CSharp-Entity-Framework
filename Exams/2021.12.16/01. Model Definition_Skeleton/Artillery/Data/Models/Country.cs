namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Common;

    public class Country
    {

        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CountryNameMaxLenght)]
        public string CountryName { get; set; } = null!;

        [Required]
        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}

