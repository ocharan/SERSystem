using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class CourseDto
  {
    [Key]
    public int CourseId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(
      @"^[0-9]{5}$",
      ErrorMessage = "El NRC debe tener 5 dígitos."
    )]
    public string Nrc { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    public string Period { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    public string Section { get; set; } = null!;
    public bool IsOpen { get; set; }
    public int? Score { get; set; }
    public int? ProfessorId { get; set; }
    public ProfessorDto? Professor { get; set; }
    public List<CourseRegistrationDto>? CourseRegistration { get; set; }
  }
}