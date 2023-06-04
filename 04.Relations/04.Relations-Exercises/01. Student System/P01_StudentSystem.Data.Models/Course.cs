namespace P01_StudentSystem.Data.Models;

using System.ComponentModel.DataAnnotations;

using Common;

public class Course
{
    public Course()
    {
        this.StudentsCourses = new HashSet<StudentCourse>();
        this.Resources = new HashSet<Resource>();
        this.Homeworks = new HashSet<Homework>();


    }

    [Key]
    public int CourseId { get; set; }

    [MaxLength(ValidationConstants.CourseNameMaxLenght)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.CourseDescriptionMaxLenght)]
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
    public virtual ICollection<Resource> Resources { get; set; }
    public virtual ICollection<Homework> Homeworks { get; set; }


}
