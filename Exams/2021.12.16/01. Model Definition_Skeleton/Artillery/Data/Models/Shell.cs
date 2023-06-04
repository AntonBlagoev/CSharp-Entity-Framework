namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Common;

    public class Shell
    {
        public Shell()
        {
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ShelCaliberMaxLenght)]
        public string Caliber { get; set; } = null!;
        public virtual ICollection<Gun> Guns { get; set; }
    }
}


