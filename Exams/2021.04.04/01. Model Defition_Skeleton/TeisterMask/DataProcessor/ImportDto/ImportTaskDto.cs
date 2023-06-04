using System.ComponentModel.DataAnnotations;
namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using TeisterMask.Common;
    using TeisterMask.Data.Models.Enums;


    [XmlType("Task")]
    public class ImportTaskDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.TaskNameMinLength)]
        [MaxLength(ValidationConstants.TaskNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; } = null!;

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; } = null!;

        [XmlElement("ExecutionType")]
        [Required]
        [Range(ValidationConstants.TaskExecutionTypeMinValue, ValidationConstants.TaskExecutionTypeMaxValue)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Required]
        [Range(ValidationConstants.TaskLabelTypeMinValue, ValidationConstants.TaskLabelTypeMaxValue)]
        public int LabelType { get; set; }

    }
}


