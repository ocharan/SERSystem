using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class CourseDto
  {
    [Key]
    public int CourseId { get; set; }
    public string Name { get; set; } = null!;
    public int Nrc { get; set; }
    public string Period { get; set; } = null!;
    public string Section { get; set; } = null!;
    public bool IsOpen { get; set; }
    public int? Score { get; set; }
    public int? ProfessorId { get; set; }
    public ProfessorDto? Professor { get; set; }
    public CourseRegistrationDto? CourseRegistration { get; set; }
  }
}