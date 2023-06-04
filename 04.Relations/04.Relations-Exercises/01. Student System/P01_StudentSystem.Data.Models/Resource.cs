namespace P01_StudentSystem.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;
using Enum;

public class Resource
{

    [Key]
    public int ResourceId { get; set; }

    [MaxLength(ValidationConstants.ResourceNameMaxLenght)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.ResourceUrlMaxLenght)]
    [Unicode(false)]
    public string? Url { get; set; }
    public ResourceType ResourceType { get; set; }

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}
