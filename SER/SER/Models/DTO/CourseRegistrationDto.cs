using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class CourseRegistrationDto
  {
    public int CourseRegistrationId { get; set; }
    public int? Score { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string RegistrationType { get; set; } = null!;
    public CourseDto Course { get; set; } = null!;
    public StudentDto Student { get; set; } = null!;
  }
}