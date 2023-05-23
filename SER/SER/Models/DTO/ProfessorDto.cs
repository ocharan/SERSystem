using System.ComponentModel.DataAnnotations;
using SER.Models.Validations.Professor;

namespace SER.Models.DTO
{
  public class ProfessorDto
  {
    [Key]
    public int ProfessorId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(100, ErrorMessage = "El nombre completo debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    public string FullName { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [ProfessorAcademicDegree(ErrorMessage = "Establezca un grado académico válido.")]
    public string AcademicDegree { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(100, ErrorMessage = "El campo debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    public string StudyField { get; set; } = null!;
    public int UserId { get; set; }
    [Required(ErrorMessage = "Campo requerido.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [StringLength(64, ErrorMessage = "El correo electrónico debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    public string Email { get; set; } = null!;
    public List<CourseDto>? Courses { get; set; }

  }
}