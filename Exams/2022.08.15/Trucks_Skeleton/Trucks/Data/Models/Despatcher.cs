namespace Trucks.Data.Models
{
    using Trucks.Common;
    using System.ComponentModel.DataAnnotations;
    public class Despatcher
    {
        public Despatcher()
        {
            this.Trucks = new HashSet<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.DespatcherNameMaxLenght)]
        public string Name { get; set; } = null!;

        public string? Position { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; }
    }
}

