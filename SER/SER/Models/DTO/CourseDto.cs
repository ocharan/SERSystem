using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class CourseDto
  {
    [Key]
    public int CourseId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(
      @"^(Proyecto Guiado|Experiencia Recepcional)$",
      ErrorMessage = "Establezca un nombre de curso válido."
    )]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(
      @"^[0-9]{5}$",
      ErrorMessage = "El NRC debe tener 5 dígitos."
    )]
    public string Nrc { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(
      @"^\d{4}-\d{2}-\d{2}\|\d{4}-\d{2}-\d{2}$",
      ErrorMessage = "Establezca un periodo válido."
    )]
    public string Period { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(
      @"^[1-3]$",
      ErrorMessage = "Establezca una sección válida."
    )]
    public string Section { get; set; } = null!;
    public bool IsOpen { get; set; }
    public int? Score { get; set; }
    public int? ProfessorId { get; set; }
    public ProfessorDto? Professor { get; set; }
    public List<CourseRegistrationDto>? CourseRegistrations { get; set; }
  }
}