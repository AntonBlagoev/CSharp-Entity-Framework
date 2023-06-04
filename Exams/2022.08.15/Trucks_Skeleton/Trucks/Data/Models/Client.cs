namespace Trucks.Data.Models
{
    using Trucks.Common;
    using System.ComponentModel.DataAnnotations;
    public class Client
    {
        public Client()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ClientNameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.ClientNationalityMaxLenght)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}

