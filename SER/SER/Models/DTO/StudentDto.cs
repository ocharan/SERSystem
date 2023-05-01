using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class StudentDto
  {
    [Key]
    public int StudentId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(
      9,
      ErrorMessage = "La matrícula debe tener {2} carácteres.",
      MinimumLength = 9
    )]
    [RegularExpression(
      @"^[Ss]\d{8}$",
      ErrorMessage = "La matrícula debe iniciar con la letra S seguido de 8 dígitos."
    )]
    public string Enrollment { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(
      100,
      ErrorMessage = "El nombre completo debe tener entre {2} y {1} carácteres.",
      MinimumLength = 10
    )]
    public string FullName { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
    public string Email { get; set; } = null!;
    public List<CourseRegistrationDto>? CourseRegistrations { get; set; }
  }
}
