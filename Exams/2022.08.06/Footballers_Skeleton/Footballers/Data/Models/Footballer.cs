namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Footballers.Common;
    using Footballers.Data.Models.Enums;
    public class Footballer
    {
        public Footballer()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstraints.FootballerNameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ContractStartDate { get; set; }

        [Required]
        public DateTime ContractEndDate { get; set; }

        [Required]
        public BestSkillType BestSkillType { get; set; }

        [Required]
        public PositionType PositionType { get; set; }

        [Required]
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }
        public virtual Coach Coach { get; set; } = null!;

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }


    }
}