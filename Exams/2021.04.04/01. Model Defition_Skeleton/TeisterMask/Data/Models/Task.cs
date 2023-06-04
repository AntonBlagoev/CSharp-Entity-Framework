namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TeisterMask.Data.Models.Enums;
    using Common;

    public class Task
    {
        public Task()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TaskNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public ExecutionType ExecutionType { get; set; }

        [Required]
        public LabelType LabelType { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;

        public ICollection<EmployeeTask> EmployeesTasks { get; set; }

    }
}


