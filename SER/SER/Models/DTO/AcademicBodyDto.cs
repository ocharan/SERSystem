using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class AcademicBodyDto
  {
    [Key]
    public int AcademicBodyId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(@"^UV-CA-\d{3}$", ErrorMessage = "Establezca una clave de cuerpo académico válida. Ejemplo: UV-CA-001")]
    public string AcademicBodyKey { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(100, ErrorMessage = "El nombre completo debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(50, ErrorMessage = "El IES debe tener entre {2} y {1} carácteres.", MinimumLength = 5)]
    public string Ies { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [RegularExpression(@"^En (Formación|Consolidación)|Consolidado$", ErrorMessage = "Establezca un grado de consolidación válido.")]
    public string ConsolidationDegree { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(100, ErrorMessage = "La longitud debe tener entre {2} y {1} carácteres.", MinimumLength = 5)]
    public string Discipline { get; set; } = null!;
    public List<AcademicBodyLgacDto>? AcademicBodyLgacs { get; set; }
    public List<AcademicBodyMemberDto>? AcademicBodyMembers { get; set; }
  }
}